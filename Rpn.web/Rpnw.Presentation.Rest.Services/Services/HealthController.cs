﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace Rpnw.Presentation.Rest.Services.Services
{
    [ApiController]
    [Route("api/HealthCheck")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            return report.Status == HealthStatus.Healthy ? Ok(report) : StatusCode(503, report);
        }
    }
}
