﻿using System.Collections.Immutable;
using LanguageExt;
using LightsOn.WebApp.Models.ClientViews;
using LightsOn.WebApp.Models.Views.Components.ClientGrid;
using LightsOn.WebApp.Services.Views.ClientViews;
using Microsoft.AspNetCore.Components;

namespace LightsOn.WebApp.Views.Components.Client;


public partial class ClientGrid : ComponentBase
{
    private const string EmptyErrorMessage = "Failed with no error message.";
    [Inject] public required IClientViewService ClientViewService { get; set; }
    public Option<ImmutableArray<ClientView>> ClientViews { get; private set; }
    public ClientGridState State { get; private set; }
    public Option<string> ErrorMessage { get; private set; }
    public ClientGrid()
    {
        ClientViews = Option<ImmutableArray<ClientView>>.None;
        ErrorMessage = Option<string>.None;
        State = ClientGridState.Loading;;
    }
    
    protected override async Task OnInitializedAsync()
    {
        var getClientViewsResult = await ClientViewService.GetClientViews();
        getClientViewsResult.Match(views =>
        {
            ClientViews = views;
            State = ClientGridState.Content;
        }, exception =>
        {
            ErrorMessage = exception.Message;
            State = ClientGridState.Error;
        });
    }
}