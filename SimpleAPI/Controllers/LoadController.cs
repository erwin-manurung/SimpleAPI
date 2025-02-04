using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Allegro.BuildTasks.Tools;
using System.Threading.Channels;
using Allegro.BuildTasks;

namespace SimpleAPI.Controllers
{
    public enum ServiceTraceEventType
    {
        Request,
        Response
    }
    public class ServiceTraceEventArgs : EventArgs
    {
        public string LegacyType => EventType.ToString();
        public string LegacyMessage { get; }
        public string ServiceName { get; }
        public string MethodName { get; }
        public ServiceTraceEventType EventType { get; }

        public IEnumerable<KeyValuePair<string, object>> ParameterMap { get; } = ClientHelper.EmptyArgMap;
        public ServiceTraceEventArgs(ServiceTraceEventType eventType, string serviceName, string method, string message)
        {
            EventType = eventType;
            ServiceName = serviceName;
            MethodName = method;
            LegacyMessage = message;
        }

        public ServiceTraceEventArgs(ServiceTraceEventType eventType, string serviceName, string method, IEnumerable<KeyValuePair<string, object>> argParmMap)
        {
            EventType = eventType;
            ServiceName = serviceName;
            MethodName = method;
            ParameterMap = argParmMap;
        }
    }
    public class CheckCacheHandler
    {
        public void OnServiceTraceEvent(object sender, ServiceTraceEventArgs e)
        {
            //log.Info($"FROM CE -> OnServiceTraceEvent{sender} ,{e.EventType}, {e.MethodName}");
        }
    }

    [ApiController]
    [Route("[controller]/[action]")]

    public class LoadController : ControllerBase
    {

        private static Dictionary<string, EventInfo> miHandlerMap = new Dictionary<string, EventInfo>();
        private static CheckCacheHandler checkHandler = new CheckCacheHandler();
        public void AttachEvent(Type type)
        {
            var eventName = "ServiceTraceEvent";
            var keyName = $"{type.Name}+{eventName}";
            EventInfo miHandler;
            if (!miHandlerMap.TryGetValue(keyName, out miHandler))
            {
                miHandler = type.GetEvent(eventName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                miHandlerMap.Add(keyName, miHandler);
            }

            MethodInfo mi = typeof(CheckCacheHandler).GetMethod($"On{eventName}");
            Delegate eventNewDelegate = Delegate.CreateDelegate(miHandler.EventHandlerType, checkHandler, mi);
            //log.Info($"[Re]Attaching Event: {eventName} on {type.Name}");
            miHandler.RemoveEventHandler(null, eventNewDelegate);
            miHandler.AddEventHandler(null, eventNewDelegate);
        }

        private readonly ApplicationPartManager _cPartManager;
        private readonly ILogger<WeatherForecastController> _logger;
        public LoadController(ApplicationPartManager partManager, ILogger<WeatherForecastController> logger)
        {
            _cPartManager = partManager;
            _logger = logger;
        }

        [HttpGet(Name = "AddController")]
        public void AddController(string assName = "ExtController.dll")
        {
            // Dynamically load assembly 
            Assembly assembly = Assembly.LoadFrom($@"d:\temp\ass\{assName}");

            // Add controller to the application
            AssemblyPart _part = new AssemblyPart(assembly);
            _cPartManager.ApplicationParts.Add(_part);
            // Notify change
            MyActionDescriptorChangeProvider.Instance.HasChanged = true;
            MyActionDescriptorChangeProvider.Instance.TokenSource.Cancel();

        }

        [HttpGet(Name = "GenController")]
        public void GenController(string classFilePath = "MainWS.asmx.cs")
        {
            RESTApiGen rESTApiGen = new RESTApiGen();
            var classContent = rESTApiGen.GenerateController(@"D:\wkspace\external\SimpleAPI\SimpleAPI\asmx\MainWS.asmx.cs");
            classContent.ToString();
        }
    }
}
