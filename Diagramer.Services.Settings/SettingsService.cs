using Diagramer.Repositories.Settings.Interfaces;
using Diagramer.Services.Settings.Core;
using Diagramer.SharedModels.Core;
using Diagramer.SharedModels.Settings;

namespace Diagramer.Services.Settings
{

    public class SettingsService : ISettingsService
    {
        private readonly IMemberAccessibilityModifiersRepository memberAccessibilityModifiersRepository;
        private readonly IMemberModifiersRepository memberModifiersRepository;
        private readonly ITypeKeywordsRepository typeKeywordsRepository;
        private readonly ITypeModifiersRepository typeModifiersRepository;
        public SettingsService(
            IMemberAccessibilityModifiersRepository memberAccessibilityModifiersRepository,
            IMemberModifiersRepository memberModifiersRepository,
            ITypeKeywordsRepository typeKeywordsRepository,
            ITypeModifiersRepository typeModifiersRepository
            )
        {
            this.memberAccessibilityModifiersRepository = memberAccessibilityModifiersRepository;
            this.memberModifiersRepository = memberModifiersRepository;
            this.typeKeywordsRepository = typeKeywordsRepository;
            this.typeModifiersRepository = typeModifiersRepository;
        }

        public List<ModifierDefinition> GetMemberAccessModifiers() =>
            memberAccessibilityModifiersRepository.GetAll().Select(x => x.data).ToList();
        public async Task<Result<bool>> UpdateMemberAccessModifiers(List<ModifierDefinition> memberAccessModifiers) =>
            await memberAccessibilityModifiersRepository.UpdateAll(memberAccessModifiers);
        
        public List<ModifierDefinition> GetMemberModifiers() =>
            memberModifiersRepository.GetAll().Select(x => x.data).ToList();
        public async Task<Result<bool>> UpdateMemberModifiers(List<ModifierDefinition> memberAccessModifiers) =>
            await memberModifiersRepository.UpdateAll(memberAccessModifiers);
        public List<ModifierDefinition> GetTypeKeywords() =>
            typeKeywordsRepository.GetAll().Select(x => x.data).ToList();
        public async Task<Result<bool>> UpdateTypeKeywords(List<ModifierDefinition> memberAccessModifiers) =>
            await typeKeywordsRepository.UpdateAll(memberAccessModifiers);
        
        public List<ModifierDefinition> GetTypeModifiers() =>
            typeModifiersRepository.GetAll().Select(x => x.data).ToList();
        public async Task<Result<bool>> UpdateTypeModifiers(List<ModifierDefinition> memberAccessModifiers) =>
            await typeModifiersRepository.UpdateAll(memberAccessModifiers);
    }
}