////using AccountingSystem.Application.Commands.Tasks;
////using AccountingSystem.Domain.Enums;
////using AccountingSystem.Domain.Interfaces;
////using MediatR;
////using TaskEntity = AccountingSystem.Domain.Entities.Task;

////namespace AccountingSystem.Application.Handlers.Tasks;

////public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Unit>
////{
////    private readonly IUnitOfWork _unitOfWork;

////    public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork)
////    {
////        _unitOfWork = unitOfWork;
////    }

////    public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
////    {
////        try
////        {
////            Console.WriteLine($"🎯 Handler נקרא! TaskId={request.TaskId}, Status={request.Status}");

////            // 1. קבל את המשימה
////            Console.WriteLine($"🔄 מחפש משימה {request.TaskId}...");
////            TaskEntity? task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);

////            if (task == null)
////            {
////                Console.WriteLine($"❌ משימה {request.TaskId} לא נמצאה!");
////                throw new Exception($"משימה עם ID {request.TaskId} לא נמצאה");
////            }

////            Console.WriteLine($"✅ משימה נמצאה: {task.Id}, סטטוס נוכחי: {task.Status}");

////            // 2. המר את הסטטוס מ-string ל-enum
////            if (!Enum.TryParse<AccountingSystem.Domain.Enums.TaskStatus>(request.Status, out var newStatus))
////            {
////                Console.WriteLine($"❌ סטטוס לא חוקי: {request.Status}");
////                throw new Exception($"סטטוס לא חוקי: {request.Status}");
////            }

////            Console.WriteLine($"✅ סטטוס חדש תקין: {newStatus}");

////            // 3. עדכן את הסטטוס
////            task.Status = newStatus;
////            task.Updatedat = DateTime.UtcNow;
////            Console.WriteLine($"🔄 מעדכן סטטוס ל-{newStatus}...");

////            // 4. אם הסטטוס הוא "הושלם", עדכן את תאריך ההשלמה
////            if (newStatus == AccountingSystem.Domain.Enums.TaskStatus.Done && !task.Completeddate.HasValue)
////            {
////                task.Completeddate = DateOnly.FromDateTime(DateTime.UtcNow);
////                Console.WriteLine($"✅ תאריך השלמה עודכן");
////            }

////            // 5. שמור
////            Console.WriteLine($"💾 שומר שינויים...");
////            await _unitOfWork.Tasks.UpdateAsync(task);
////            await _unitOfWork.SaveChangesAsync(cancellationToken);

////            Console.WriteLine($"✅ שינויים נשמרו בהצלחה!");
////            return Unit.Value;
////        }
////        catch (Exception ex)
////        {
////            Console.WriteLine($"❌ שגיאה כללית: {ex.Message}");
////            Console.WriteLine($"❌ Inner Exception: {ex.InnerException?.Message}");
////            Console.WriteLine($"❌ Full Stack: {ex}");
////            throw;
////        }
////    }
////}
//using AccountingSystem.Application.Commands.Tasks;
//using AccountingSystem.Domain.Enums;
//using AccountingSystem.Domain.Interfaces;
//using MediatR;
//using TaskEntity = AccountingSystem.Domain.Entities.Task;

//namespace AccountingSystem.Application.Handlers.Tasks;

//public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Unit>
//{
//    private readonly IUnitOfWork _unitOfWork;

//    public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork)
//    {
//        _unitOfWork = unitOfWork;
//    }

//    public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            Console.WriteLine($"🎯 Handler נקרא! TaskId={request.TaskId}, Status={request.Status}");

//            // 1. קבל את המשימה
//            TaskEntity? task = await _unitOfWork.Tasks.GetByIdAsync(request.TaskId);

//            if (task == null)
//            {
//                throw new Exception($"משימה עם ID {request.TaskId} לא נמצאה");
//            }

//            // 2. המר את הסטטוס מ-string ל-enum
//            if (!Enum.TryParse<AccountingSystem.Domain.Enums.TaskStatus>(request.Status, out var newStatus))
//            {
//                throw new Exception($"סטטוס לא חוקי: {request.Status}");
//            }

//            // 3. עדכן את הסטטוס
//            task.Status = newStatus;
//            task.Updatedat = DateTime.Now;  // ← שינוי כאן!

//            // 4. אם הסטטוס הוא "הושלם", עדכן את תאריך ההשלמה
//            if (newStatus == AccountingSystem.Domain.Enums.TaskStatus.Done && !task.Completeddate.HasValue)
//            {
//                task.Completeddate = DateOnly.FromDateTime(DateTime.Now);  // ← שינוי כאן!
//            }

//            // 5. שמור
//            await _unitOfWork.Tasks.UpdateAsync(task);
//            await _unitOfWork.SaveChangesAsync(cancellationToken);

//            Console.WriteLine($"✅ שינויים נשמרו בהצלחה!");
//            return Unit.Value;
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"❌ שגיאה: {ex.Message}");
//            throw;
//        }
//    }
//}
using AccountingSystem.Application.Commands.Tasks;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using MediatR;
using System.ComponentModel;

namespace AccountingSystem.Application.Handlers.Tasks;

public class UpdateTaskStatusCommandHandler : IRequestHandler<UpdateTaskStatusCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTaskStatusCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine($"🎯 Handler נקרא! TaskId={request.TaskId}, Status={request.Status}");

            // 1. וודא שהמשימה קיימת
            var taskExists = await _unitOfWork.Tasks.ExistsAsync(request.TaskId);
            if (!taskExists)
            {
                throw new Exception($"משימה עם ID {request.TaskId} לא נמצאה");
            }

            // 2. וולידציה על הסטטוס (אופציונלי)
            var validStatuses = new[] { "Pending", "InProgress", "Done", "Paid", "NotRequired" };
            if (!validStatuses.Contains(request.Status, StringComparer.OrdinalIgnoreCase))
            {
                throw new Exception($"סטטוס לא חוקי: {request.Status}");
            }

            Console.WriteLine($"✅ סטטוס תקין: {request.Status}");

            // 3. עדכן דרך UnitOfWork
            var rowsAffected = await _unitOfWork.UpdateTaskStatusAsync(
                request.TaskId,
                request.Status,
                cancellationToken
            );

            if (rowsAffected == 0)
            {
                throw new Exception($"לא הצלחנו לעדכן את המשימה {request.TaskId}");
            }

            Console.WriteLine($"✅ שינויים נשמרו בהצלחה!");
            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ שגיאה: {ex.Message}");
            throw;
        }
    }
}
