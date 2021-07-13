using Common.DataModel.Domain.Logger;
using System;
using System.Runtime.CompilerServices;

namespace Logger
{
    public class Logger
    {
        LoggerDao loggerDao = new LoggerDao();
        private void Log(LogDto logDto)
        {
            loggerDao.Create(logDto);
        }

        public void Debug(string message, long? duration, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLine = -1)
        {
            string callsite = $"Caller Name: {callerName}, {Environment.NewLine} Caller FilePath: {callerFilePath}, {Environment.NewLine} Caller Line number: {callerLine}";

            Log(new LogDto()
            {
                Callsite = callsite,
                Message = message,
                Duration = duration,
                CreatedAt = DateTime.Now,
                Level = Level.Debug
            });
        }

        public void Warning(string message, Exception exception, long? duration, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLine = -1)
        {
            string callsite = $"Caller Name: {callerName}, {Environment.NewLine} Caller FilePath: {callerFilePath}, {Environment.NewLine} Caller Line number: {callerLine}";

            Log(new LogDto()
            {
                Callsite = callsite,
                Message = message,
                Duration = duration,
                CreatedAt = DateTime.Now,
                Level = Level.Warning,
                Exception = exception.ToString()
            });
        }

        public void Error(string message, Exception exception, long? duration, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLine = -1)
        {
            string callsite = $"Caller Name: {callerName}, {Environment.NewLine} Caller FilePath: {callerFilePath}, {Environment.NewLine} Caller Line number: {callerLine}";

            Log(new LogDto()
            {
                Callsite = callsite,
                Message = message,
                Duration = duration,
                CreatedAt = DateTime.Now,
                Level = Level.Error,
                Exception = exception.ToString()
            });
        }

        public void Fatal(Exception exception, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLine = -1, params Object[] request)
        {
            string callsite = $"Caller Name: {callerName}, {Environment.NewLine} Caller FilePath: {callerFilePath}, {Environment.NewLine} Caller Line number: {callerLine}";
            string message = new RequestResponseDto(null, request).ToString();

            Log(new LogDto()
            {
                Callsite = callsite,
                CreatedAt = DateTime.Now,
                Level = Level.Fatal,
                Message = message,
                Exception = exception.ToString()
            });
        }

        public void Verbose(string message, long? duration, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLine = -1)
        {
            string callsite = $"Caller Name: {callerName}, {Environment.NewLine} Caller FilePath: {callerFilePath}, {Environment.NewLine} Caller Line number: {callerLine}";

            Log(new LogDto()
            {
                Callsite = callsite,
                Message = message,
                Duration = duration,
                CreatedAt = DateTime.Now,
                Level = Level.Verbose,
            });
        }

        public void Verbose(long? duration, object response, [CallerMemberName] string callerName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLine = -1, params Object[] request)
        {
            string message = new RequestResponseDto(response, request).ToString();
            string callsite = $"Caller Name: {callerName}, {Environment.NewLine} Caller FilePath: {callerFilePath}, {Environment.NewLine} Caller Line number: {callerLine}";

            Log(new LogDto()
            {
                Callsite = callsite,
                Message = message,
                Duration = duration,
                CreatedAt = DateTime.Now,
                Level = Level.Verbose,
            });
        }
    }
}