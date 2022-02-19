using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.IO;

public class LogHelper
  {
      private static ILoggerRepository repository { get; set; }
      private static ILog _log;
      private static ILog log {
          get
          {
              if (_log == null)
              {
                  Configure();
              }
              return _log;
          }
      }
      public static void Configure(string repositoryName = "NETCoreRepository", string configFile = "log4net.config")
      {
          repository = LogManager.CreateRepository(repositoryName);
          XmlConfigurator.Configure(repository, new FileInfo(configFile));
          _log = LogManager.GetLogger(repositoryName, "");
      }

      public static void Info(string msg)
      {
          log.Info(msg);
      }

      public static void Warn(string msg, Exception ex)
      {
          log.Warn(msg, ex);
      }

      public static void Error(string msg, Exception ex)
      {
          log.Error(msg, ex);
      }
  }