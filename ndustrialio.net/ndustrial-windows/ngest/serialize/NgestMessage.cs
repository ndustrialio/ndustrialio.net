using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace com.ndustrialio.api.ngest.serialize
{
    // This little class helps a lot with serialization
    public class NgestMessage
    {

        public static int MAX_MESSAGE_FIELDS = 50;

        public static String TIMESERIES_TYPE = "timeseries";

        public String _type;
        // Timestamp->fields mapping
        public Dictionary<String, TimeSeriesDataObject> _data;
        public String _feedKey;
        public int _fields;

        public NgestMessage(String type, String feedKey)
        {
            _data = new Dictionary<String, TimeSeriesDataObject>();
            _feedKey = feedKey;
            _type = type;
            _fields = 0;
        }

        public bool addData(String timestamp, String field, Object value)
        {
            if (_fields < MAX_MESSAGE_FIELDS)
            {
                // Add data to this NgestMessage
                TimeSeriesDataObject t = _data[timestamp];

                // Does a TimeSeriesDataObject exist at this timestamp in this message?
                if (t == null)
                {
                    // No, create one and add
                    t = new TimeSeriesDataObject();

                    t.addValue(field, value);

                    _data[timestamp] = t;
                }
                else
                {
                    // Yes
                    t.addValue(field, value);
                }

                _fields++;

                // Indicate data accepted
                return true;
            }
            else
            {
                // Indicate data not accepted
                return false;
            }

        }

        public int Fields
        {
            // Number of fields in this message
            get { return _fields; }
        }

        public override string ToString()
        {

            // Serialize this message to JSON by hand
            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);

            // { -- begin message JSON
            writer.WriteStartObject();

            // "type": "<type>" -- write type property
            writer.WritePropertyName("type");
            writer.WriteValue(_type);

            // "data": [ -- Write data property and begin overall data array
            writer.WritePropertyName("data");
            writer.WriteStartArray();


            foreach (KeyValuePair<String, TimeSeriesDataObject> timePoint in _data)
            {
                // { -- Write time point object start
                writer.WriteStartObject();

                // "timestamp": "<timestamp>" -- write timestamp property
                writer.WritePropertyName("timestamp");
                writer.WriteValue(timePoint.Key);
                
                // "data": { -- write data property and data object start
                writer.WritePropertyName("data");
                writer.WriteStartObject();

                foreach (KeyValuePair<String, String> fields in timePoint.Value.Data)
                {
                    // "<field_name>": -- write field name property
                    writer.WritePropertyName(fields.Key);
                    
                    // {"value": <field_value>}" -- write small value object
                    writer.WriteStartObject();
                    writer.WritePropertyName("value");
                    writer.WriteValue(fields.Value);
                    writer.WriteEndObject();
                }

                // } -- write data object end
                writer.WriteEndObject();

                // } -- write time point object end
                writer.WriteEndObject();
            }

            // ] -- end overall data array
            writer.WriteEndArray();

            // "feedKey": "<feed_key>" -- write feed key property
            writer.WritePropertyName("feedKey");
            writer.WriteValue(_feedKey);

            // } -- end message JSON
            writer.WriteEndObject();

            return sw.ToString();
        }
    }
}
