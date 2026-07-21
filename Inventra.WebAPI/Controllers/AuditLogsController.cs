using Inventra.Application.Interfaces;
using Inventra.Application.DTOs;
using Inventra.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Inventra.WebAPI.Controllers
{
    /// <summary>
    /// API endpoints for audit log queries.
    /// Provides read-only access to audit logs for compliance and monitoring.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditLogsController> _logger;

        public AuditLogsController(IAuditService auditService, ILogger<AuditLogsController> logger)
        {
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets audit history for a specific entity.
        /// </summary>
        /// <param name="entityType">Full type name of the entity.</param>
        /// <param name="entityId">Primary key of the entity.</param>
        /// <returns>Audit history for the entity.</returns>
        [HttpGet("entity-history")]
        public async Task<IActionResult> GetEntityHistory(string entityType, string entityId)
        {
            var logs = await _auditService.GetEntityAuditHistoryAsync(entityType, entityId);
            var dtos = logs.Select(MapToDto).ToList();

            return Ok(new AuditHistoryDto
            {
                EntityType = entityType,
                EntityId = entityId,
                AuditLogs = dtos
            });
        }

        /// <summary>
        /// Gets audit logs for a specific action type.
        /// </summary>
        /// <param name="action">Action type to filter by.</param>
        /// <param name="limit">Maximum number of results.</param>
        /// <returns>Collection of audit logs.</returns>
        [HttpGet("by-action")]
        public async Task<IActionResult> GetByAction(AuditActionType action, int limit = 100)
        {
            var logs = await _auditService.GetAuditLogsByActionAsync(action, limit);
            var dtos = logs.Select(MapToDto).ToList();

            return Ok(dtos);
        }

        /// <summary>
        /// Gets audit logs for a specific user.
        /// </summary>
        /// <param name="userId">User ID to filter by.</param>
        /// <param name="limit">Maximum number of results.</param>
        /// <returns>Collection of audit logs.</returns>
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId, int limit = 100)
        {
            var logs = await _auditService.GetUserAuditHistoryAsync(userId, limit);
            var dtos = logs.Select(MapToDto).ToList();

            return Ok(dtos);
        }

        /// <summary>
        /// Gets audit logs within a date range.
        /// </summary>
        /// <param name="startDate">Start date (ISO 8601 format).</param>
        /// <param name="endDate">End date (ISO 8601 format).</param>
        /// <param name="limit">Maximum number of results.</param>
        /// <returns>Collection of audit logs.</returns>
        [HttpGet("by-date-range")]
        public async Task<IActionResult> GetByDateRange(DateTime startDate, DateTime endDate, int limit = 1000)
        {
            var logs = await _auditService.GetAuditLogsByDateRangeAsync(startDate, endDate, limit);
            var dtos = logs.Select(MapToDto).ToList();

            return Ok(dtos);
        }

        private static AuditLogDto MapToDto(Domain.Entities.AuditLog log)
        {
            return new AuditLogDto
            {
                Id = log.Id,
                EntityType = log.EntityType,
                EntityId = log.EntityId,
                Action = log.Action,
                ActionTimestamp = log.ActionTimestamp,
                UserDisplay = log.GetUserDisplay(),
                IpAddress = log.IpAddress,
                ChangedProperties = log.ChangedProperties,
                HasSensitiveChanges = log.HasSensitiveChanges()
            };
        }
    }
}