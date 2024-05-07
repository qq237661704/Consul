namespace Microsoft.Extensions.DependencyInjection
{
    public class ConsulServiceOptions
    {
        /// <summary>
        /// 当前服务部署的地址
        /// </summary>
        public string ServiceIP { get; set; } = "127.0.0.1";

        /// <summary>
        /// 端口
        /// </summary>
        public int ServicePort { get; set; }

        /// <summary>
        /// 当前服务的名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// http://localhost:8500
        /// </summary>
        public string Address { get; set; } = "http://localhost:8500";
    }
}
