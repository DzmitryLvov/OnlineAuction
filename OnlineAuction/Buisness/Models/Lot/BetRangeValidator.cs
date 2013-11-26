using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using OnlineAuction.Buisness.Data;
using OnlineAuction.Buisness.Models.Lot;

public class BetRangeValidator : ValidationAttribute, IClientValidatable
{
    private readonly string _minPropertyName;

    public BetRangeValidator(string minPropertyName)
    {
        _minPropertyName = minPropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
       /* var dataAccess = new DataAccess();
        var model = (LotViewModel)validationContext.ObjectInstance;
        if (model.ID > 0)
        {
            var minValue = dataAccess.GetViewModelById(model.ID).Currency;
            var currentValue = Int64.Parse(value.ToString());
            if (currentValue <= minValue)
            {

                return new ValidationResult(
                    string.Format(
                        ErrorMessage,
                        minValue
                        )
                    );
            }
        }
        */
        return null;
    }

    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
        var rule = new ModelClientValidationRule
        {
            ValidationType = "betrange",
            ErrorMessage = this.ErrorMessage,
        };
        rule.ValidationParameters["minvalueproperty"] = _minPropertyName;
        yield return rule;
    }
}