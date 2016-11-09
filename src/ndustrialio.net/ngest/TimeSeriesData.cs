using System;
using System.Collections.Generic;
using com.ndustrialio.api.ngest.serialize;


namespace com.ndustrialio.api.ngest
{
    public class TimeSeriesData
    {
        private String _feedKey;

        private TimeZoneInfo _feedTimeZone;

        public static String TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private List<NgestMessage> _messages;

        private NgestMessage _currentMessage;

        public TimeSeriesData(String feed_key, TimeZoneInfo feed_timezone)
        {
            _feedKey = feed_key;

            _feedTimeZone = feed_timezone;

            _messages = new List<NgestMessage>();

            _currentMessage = new NgestMessage(NgestMessage.TIMESERIES_TYPE, _feedKey);
            _messages.Add(_currentMessage);
        }

        public void addValue(DateTime timestamp, String field, Object value)
        {
            DateTime delocalized;


            // Delocalize timestamp, if it's not already UTC
            if (_feedTimeZone != TimeZoneInfo.Utc)
            {
                delocalized = TimeZoneInfo.ConvertTime(timestamp, _feedTimeZone);
            } else
            {
                delocalized = timestamp;
            }

            // Stringify timestamps.. basically
            String delocalizedStr = delocalized.ToString(TIMESTAMP_FORMAT);

            try
            {
                // Try adding the data point to the current message
                bool accepted = _currentMessage.addData(
                                        delocalizedStr,
                                        field, value);

                if (!accepted)
                {
                    // Current message is full!

                    // Create new message
                    _currentMessage = new NgestMessage(NgestMessage.TIMESERIES_TYPE, _feedKey);

                    // Add data.. no way for this to fail
                    _currentMessage.addData(delocalizedStr, field, value);

                    // Add to messages list
                    _messages.Add(_currentMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Field ID: " + field + " already exists at: "
                    + timestamp.ToString(TIMESTAMP_FORMAT));
            }
        }

        public List<String> getJSONData()
        {
            List<String> ret = new List<String>();

            // Return ngest messages as string
            foreach(var m in _messages)
            {
                ret.Add(m.ToString());
            }

            return ret;
        }
    }
}
