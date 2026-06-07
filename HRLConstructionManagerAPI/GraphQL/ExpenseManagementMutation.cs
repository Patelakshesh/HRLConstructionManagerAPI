namespace HRLConstructionManagerAPI.GraphQL;

using HRLConstructionManagerAPI.Entities;
using HRLConstructionManagerAPI.Models;
using HRLConstructionManagerAPI.Repositories;
using HotChocolate.Authorization;

[ExtendObjectType(OperationTypeNames.Mutation)]
public sealed class ExpenseManagementMutation
{
    [Authorize]
    public async Task<Expense> CreateExpense(
        CreateExpenseInput input,
        [Service] IExpenseRepository expenseRepository)
    {
        var expense = new Expense
        {
            Title = input.Title.Trim(),
            SiteId = input.SiteId,
            CategoryId = input.CategoryId,
            Amount = input.Amount,
            PaymentMode = input.PaymentMode.Trim(),
            TransactionId = input.TransactionId?.Trim(),
            Date = input.Date,
            Type = input.Type.Trim(),
            CreatedBy = input.CreatedBy,
            ReceiptImage = input.ReceiptImage
        };

        return await expenseRepository.CreateExpenseAsync(expense);
    }

    [Authorize]
    public async Task<Expense?> UpdateExpense(
        UpdateExpenseInput input,
        [Service] IExpenseRepository expenseRepository)
    {
        var expense = await expenseRepository.GetExpenseByIdAsync(input.Id);
        if (expense == null)
        {
            return null;
        }

        expense.Title = input.Title.Trim();
        expense.SiteId = input.SiteId;
        expense.CategoryId = input.CategoryId;
        expense.Amount = input.Amount;
        expense.PaymentMode = input.PaymentMode.Trim();
        expense.TransactionId = input.TransactionId?.Trim();
        expense.Date = input.Date;
        expense.Type = input.Type.Trim();
        expense.ModifiedBy = input.ModifiedBy;
        expense.ModifiedOn = DateTime.UtcNow;
        expense.ReceiptImage = input.ReceiptImage;

        return await expenseRepository.UpdateExpenseAsync(expense);
    }

    [Authorize]
    public async Task<bool> DeleteExpense(
        int id,
        [Service] IExpenseRepository expenseRepository)
    {
        return await expenseRepository.DeleteExpenseAsync(id);
    }
}
