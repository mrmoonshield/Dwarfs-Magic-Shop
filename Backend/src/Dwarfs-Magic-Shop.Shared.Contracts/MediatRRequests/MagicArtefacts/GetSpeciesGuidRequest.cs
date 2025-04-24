using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using MediatR;

namespace Dwarfs_Magic_Shop.Shared.Contracts.MediatRRequests.MagicArtefacts;

public record GetSpeciesGuidRequest(string Species) : IRequest<Result<Guid, ErrorsList>>;
