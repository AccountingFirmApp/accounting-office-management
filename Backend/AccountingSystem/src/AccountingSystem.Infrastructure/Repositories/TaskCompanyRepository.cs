﻿using AccountingSystem.Domain.Entities;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccountingSystem.Infrastructure.Repositories
{
    public class CompanyTaskRepository : ICompanyTaskRepository
    {
        private readonly AccountingDbContext _context;
        private readonly DbSet<CompanyTask> _dbSet;

        public CompanyTaskRepository(AccountingDbContext context)
        {
            _context = context;
            _dbSet = context.CompanyTasks;
        }


        public async Task<CompanyTask?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .Include(t => t.ChecklistItems)
                    .ThenInclude(i => i.CompletedByWorker)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<CompanyTask>> GetAllAsync()
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> FindAsync(Expression<Func<CompanyTask, bool>> predicate)
        {
            return await _dbSet
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task AddAsync(CompanyTask entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CompanyTask entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task DeleteByCompanyIdAsync(int companyId)
        {
            var tasks = await _context.CompanyTasks
                .Where(t => t.Companyid == companyId)
                .ToListAsync();

            _context.CompanyTasks.RemoveRange(tasks);
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(t => t.Id == id);
        }

        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        // ==================== שאילתות ספציפיות למשימות ====================

        public async Task<IEnumerable<CompanyTask>> GetTasksByCompanyIdAsync(int companyId)
        {
            return await _dbSet
        .Where(t => t.Companyid == companyId && t.Isactive == true)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .OrderBy(t => t.Duedate)
                .ToListAsync();
        }
        public async Task<IEnumerable<CompanyTask>> GetTasksByWorkerIdAsync(int workerId)
        {
            var monthAgo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-30));

            return await _context.CompanyTasks
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .Where(t =>
                    // 1. שיוך לעובד או לחברה
                    (t.Assignedworkerid == workerId || t.Company.Companyworkers.Any(cw => cw.Workerid == workerId))
                    &&
                    // 2. סינון משימות רלוונטיות
                    (
                        // שימוש בטיפוס ה-Enum המקורי במקום Cast ל-int
                        t.Status != TaskStatus1.Done ||

                        // משימות שהושלמו אבל הן מהחודש האחרון
                        (t.Duedate == null || t.Duedate >= monthAgo)
                    )
                )
                .OrderByDescending(t => t.Createdat)
                .ToListAsync();
        }
        



        public async Task<IEnumerable<CompanyTask>> GetTasksByStatusAsync(string status)
        {
            return await _dbSet
                .Where(t => t.Status.ToString() == status)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByTaskTypeIdAsync(int taskTypeId)
        {
            return await _dbSet
                .Where(t => t.Tasktypeid == taskTypeId)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByPeriodAsync(DateTime period)
        {
            var periodDateOnly = DateOnly.FromDateTime(period);
            return await _dbSet
                .Where(t => t.Period == periodDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);
            var endDateOnly = DateOnly.FromDateTime(endDate);

            return await _dbSet
                .Where(t => t.Period >= startDateOnly && t.Period <= endDateOnly)
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetOverdueTasksAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return await _dbSet
                .Where(t => t.Duedate < today && t.Status.ToString() == "Pending")
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetTasksDueInNextDaysAsync(int days)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            var futureDate = today.AddDays(days);

            return await _dbSet
                .Where(t => t.Duedate >= today && t.Duedate <= futureDate && t.Status.ToString() == "Pending")
                .Include(t => t.Company)
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .ToListAsync();
        }

        public async Task<IEnumerable<CompanyTask>> GetPendingTasksAsync()
        {
            return await GetTasksByStatusAsync("Pending");
        }

        // ==================== פעולות ליצירה אוטומטית ====================

       

           
        public async Task<TaskTypeConfiguration?> GetConfigurationByTaskTypeAsync(int taskTypeId)
        {
            return await _context.TaskTypeConfiguration
                .FirstOrDefaultAsync(c => c.TaskTypeId == taskTypeId && c.IsActive);
        }

        public async Task<List<Company>> GetActiveCompaniesAsync()
        {
            return await _context.Companies
                .Where(c => c.Isactive)
                .ToListAsync();
        }

       

        public async Task<bool> TaskExistsAsync(int companyId, int taskTypeId, DateOnly period)
        {
            return await _context.CompanyTasks
                .AnyAsync(t =>
                    t.Companyid == companyId &&
                    t.Tasktypeid == taskTypeId &&
                    t.Period == period);
        }

        public async Task<List<CompanyTask>> GetCompanyTasksAsync(
            int companyId,
            int? year = null,
            int? month = null,
            TaskStatus1? status = null)
        {
            var query = _context.CompanyTasks
                .Include(t => t.Tasktype)
                .Include(t => t.Assignedworker)
                .Include(t => t.ChecklistItems)
                .Where(t => t.Companyid == companyId);

            if (year.HasValue && month.HasValue)
            {
                var period = new DateOnly(year.Value, month.Value, 1);
                query = query.Where(t => t.Period == period);
            }
            else if (year.HasValue)
            {
                query = query.Where(t => t.Period.Year == year.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            return await query
                .OrderByDescending(t => t.Period)
                .ThenBy(t => t.Duedate)
                .ToListAsync();
        }

        public async Task<int> CreateTasksAsync(List<CompanyTask> tasks)
        {
            _context.CompanyTasks.AddRange(tasks);
            return await _context.SaveChangesAsync();
        }

        // ==================== Checklist ====================

        public async Task<TaskChecklistTemplate?> GetActiveChecklistTemplateAsync(int taskTypeId)
        {
            var template = await _context.TaskChecklistTemplate
                .FirstOrDefaultAsync(t => t.TaskTypeId == taskTypeId && t.IsActive && t.AutoCreateItems);

            if (template != null)
            {
                await _context.Entry(template)
                    .Collection(t => t.Items)
                    .LoadAsync();

                template.Items = template.Items.OrderBy(i => i.OrderIndex).ToList();
            }

            return template;
        }

        public async Task<List<CompanyTaskChecklistItem>> GetTaskChecklistItemsAsync(int taskId)
        {
            return await _context.CompanyTaskChecklistItems
                .Include(i => i.CompletedByWorker)
                .Include(i => i.TemplateItem)
                .Where(i => i.CompanyTaskId == taskId)
                .OrderBy(i => i.OrderIndex)
                .ToListAsync();
        }

        public async Task<CompanyTaskChecklistItem?> GetChecklistItemByIdAsync(int itemId)
        {
            return await _context.CompanyTaskChecklistItems
                .Include(i => i.CompanyTask)
                .FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task UpdateChecklistItemAsync(CompanyTaskChecklistItem item)
        {
            _context.CompanyTaskChecklistItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<CompanyTaskChecklistItem> AddChecklistItemAsync(
            CompanyTaskChecklistItem item)
        {
            _context.CompanyTaskChecklistItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteChecklistItemAsync(int itemId)
        {
            var item = await _context.CompanyTaskChecklistItems.FindAsync(itemId);

            if (item != null)
            {
                _context.CompanyTaskChecklistItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TaskChecklistTemplate> GetChecklistTemplateByTaskTypeAsync(int taskTypeId)
        {
            return await _context.TaskChecklistTemplates
                .Include(t => t.Items) // חשוב מאוד כדי לקבל את רשימת השלבים
                .FirstOrDefaultAsync(t => t.TaskTypeId == taskTypeId && t.IsActive);
        }

        public async Task UpdateChecklistTemplateAsync(TaskChecklistTemplate template, List<TaskChecklistTemplateItem> newItems)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // עדכון או הוספת התבנית
                if (template.Id == 0) _context.TaskChecklistTemplates.Add(template);
                else _context.TaskChecklistTemplates.Update(template);

                await _context.SaveChangesAsync();

                // החלפת הפריטים
                var oldItems = _context.TaskChecklistTemplateItems.Where(i => i.TemplateId == template.Id);
                _context.TaskChecklistTemplateItems.RemoveRange(oldItems);

                foreach (var item in newItems)
                {
                    item.TemplateId = template.Id;
                    _context.TaskChecklistTemplateItems.Add(item);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Tasktype>> GetTaskTypesAsync()
        {
            return await _context.Tasktypes
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<CompanyTaskConfiguration?> GetCompanySettingsAsync(int companyId, int taskTypeId)
        {
            return await _context.CompanyTaskConfigurations
                .FirstOrDefaultAsync(c => c.Companyid == companyId && c.Tasktypeid == taskTypeId);
        }

        public async Task<List<TaskTypeConfiguration>> GetActiveConfigurationsByRecurrenceAsync(RecurrenceType recurrence)
        {
            return await _context.TaskTypeConfiguration
                .Where(t => t.RecurrenceType == recurrence && t.IsActive)
                .ToListAsync();}

        public async Task SoftDeleteByCompanyIdAsync(int companyId)
        {
            var tasks = await _context.CompanyTasks
                .Where(t => t.Companyid == companyId)
                .ToListAsync();

            foreach (var task in tasks)
                task.Isactive = false;
        }
    }
    }
    