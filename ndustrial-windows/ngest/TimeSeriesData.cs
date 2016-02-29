using System;
using System.Collections.Generic;
using com.ndustrialio.api.ngest.serialize;
using com.ndustrialio.api.errors;


namespace com.ndustrialio.api.ngest
{
    public class TimeSeriesData
    {
        private String _feedKey, _feedTimeZone;

        public static String TIMESTAMP_FORMAT = "yyyy-MM-dd HH:mm:ss";

        private List<NgestMessage> _messages;

        private NgestMessage _currentMessage;

        public TimeSeriesData(String feed_key, String feed_timezone)
        {
            _feedKey = feed_key;

            _feedTimeZone = feed_timezone;

            _messages = new List<NgestMessage>();

            _currentMessage = new NgestMessage(NgestMessage.TIMESERIES_TYPE, _feedKey);
            _messages.Add(_currentMessage);
        }

        public void addValue(DateTime timestamp, String field, Object value)
        {
            // Delocalize timestamp
            DateTime delocalized = delocalizeTimestamp(timestamp);

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
            catch (NonUniqueFieldIDException e)
            {
                Console.WriteLine("Field ID: " + field + " already exists at: "
                    + timestamp.ToString(TIMESTAMP_FORMAT));
            }
        }

        private DateTime delocalizeTimestamp(DateTime timestamp)
        {
            // Look up timezone in system
            TimeZoneInfo fromTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_feedTimeZone);

            // Return delocalized timestamp
            return TimeZoneInfo.ConvertTimeToUtc(timestamp, fromTimeZone);
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
