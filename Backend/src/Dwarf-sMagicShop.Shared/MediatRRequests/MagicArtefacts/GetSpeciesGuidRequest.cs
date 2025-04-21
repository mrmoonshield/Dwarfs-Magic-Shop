using CSharpFunctionalExtensions;
using Dwarf_sMagicShop.Core.ErrorsHelpers;
using MediatR;

namespace Dwarf_sMagicShop.Core.MediatRRequests.MagicArtefacts;

public record GetSpeciesGuidRequest(string Species) : IRequest<Result<Guid, ErrorsList>>;
