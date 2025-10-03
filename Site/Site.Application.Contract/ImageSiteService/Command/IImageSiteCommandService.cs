
using Shared.Application;

namespace Site.Application.Contract.ImageSiteService.Command
{
	public interface IImageSiteCommandService
	{
        Task<OperationResult> Create(CreateImageSiteCommandModel command);
		Task<OperationResult> DeleteFromDataBase(int id);
	}
}
