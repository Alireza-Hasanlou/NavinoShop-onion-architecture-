using Azure;
using Shared;
using Shared.Application;
using Shared.Application.Validations;
using Site.Application.Contract.SitePageService.Command;
using Site.Domain.SitePageAgg;


namespace Site.Application.Services;

internal class SitePageService : ISitePageCommandService
{
	private readonly ISitePageRepository _sitePageRepository;

	public SitePageService(ISitePageRepository sitePageRepository)
	{
		_sitePageRepository = sitePageRepository;
	}

    public async Task<OperationResult> ActivationChangeAsync(int id)
	{
		var page = await _sitePageRepository.GetByIdAsync(id);
		page.ActivationChange();
	   if(	await _sitePageRepository.SaveAsync())
			return new(true);
	   return new(false);
	}

    public async Task<OperationResult> CreateAsync(CreateSitePageCommnadModel command)
	{
		if (await _sitePageRepository.ExistByAsync(c => c.Title == command.Title.Trim()))
			return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
		var slug = SlugUtility.GenerateSlug(command.Slug);
		if (await _sitePageRepository.ExistByAsync(c => c.Slug == slug))
			return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Slug));

		SitePage page = new(command.Title.Trim(), slug, command.Text);
		var result= await _sitePageRepository.CreateAsync(page);
		if (result.Success) return new(true);
		return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
	}

	public async Task<OperationResult> EditAsync(EditSitePageCommandModel command)
	{
		var page =await _sitePageRepository.GetByIdAsync(command.Id);
		if (await _sitePageRepository.ExistByAsync(c => c.Title == command.Title.Trim() && c.Id != page.Id))
			return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Title));
		var slug = SlugUtility.GenerateSlug(command.Slug);
		if (await _sitePageRepository.ExistByAsync(c => c.Slug == slug && c.Id != page.Id))
			return new(false, ValidationMessages.DuplicatedMessage, nameof(command.Slug));

		 page.Edit(command.Title.Trim(), slug, command.Text);
		if (await _sitePageRepository.SaveAsync()) return new(true);
		return new(false, ValidationMessages.SystemErrorMessage, nameof(command.Title));
	}

	public async Task<EditSitePageCommandModel> GetForEditAsync(int id) =>
		await _sitePageRepository.GetForEdit(id);
}
