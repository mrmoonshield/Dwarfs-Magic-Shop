﻿namespace FileService.Endpoints;

public interface IEndpoint
{
	void MapEndpoint(IEndpointRouteBuilder app);
}