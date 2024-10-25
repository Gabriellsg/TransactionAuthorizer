﻿using TransactionAuthorizer.Domain.Entities;

namespace TransactionAuthorizer.Domain.Interfaces;

public interface IBenefitCategoryRepository
{
    Task<BenefitCategoryDomain?> GetBenefitCategoryAsync(string accountNumber);
    Task<BenefitCategoryDomain> GetDefaultBenefitCategoryAsync();
}