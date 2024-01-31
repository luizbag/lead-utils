using FluentValidation;
using Utils.Daily;

namespace utils.Daily
{
    public class RandomDailyConfigurationValidator : AbstractValidator<RandomDailyConfiguration>
    {
        public RandomDailyConfigurationValidator()
        {
            RuleFor(config => config.Teams).NotEmpty();
            RuleForEach(config => config.Teams).SetValidator(new RandomDailyTeamValidator());
            RuleFor(config => config.Notes).NotNull().SetValidator(new RandomDailyNotesValidator());
        }
    }

    public class RandomDailyNotesValidator : AbstractValidator<RandomDailyNotes>
    {
        public RandomDailyNotesValidator()
        {
            RuleFor(notes => notes.FilePath).NotEmpty();
            RuleFor(notes => notes.WriteMode).NotEmpty();
        }
    }

    public class RandomDailyTeamValidator : AbstractValidator<RandomDailyTeam>
    {
        public RandomDailyTeamValidator()
        {
            RuleFor(team => team.Name).NotEmpty();
            RuleFor(team => team.Members).NotEmpty();
            RuleForEach(team => team.Members).NotEmpty();
        }
    }
}