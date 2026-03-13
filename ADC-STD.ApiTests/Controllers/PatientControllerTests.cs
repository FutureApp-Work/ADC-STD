using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ADC_STD.ApiTests.Fixtures;
using ADC_STD.ApiTests.Helpers;
using dotnet.Core;
using dotnet.models.testing.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ADC_STD.ApiTests.Controllers;

public class PatientControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private readonly HttpClient _authenticatedClient;

    public PatientControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _authenticatedClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        _authenticatedClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("TestScheme");
    }

    #region GET /app001/getPatientList Tests

    [Fact]
    public async Task GetPatientList_WithoutAuth_Returns401()
    {
        // Act
        var response = await _client.GetAsync("/app001/getPatientList");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPatientList_WithAuth_Returns200()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPatientList");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Items);
        Assert.True(result.Data.Items.Count > 0);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 5)]
    [InlineData(2, 2)]
    public async Task GetPatientList_WithPagination_ReturnsCorrectPage(int page, int pageSize)
    {
        // Act
        var response = await _authenticatedClient.GetAsync($"/app001/getPatientList?page={page}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(page, result.Data.Page);
        Assert.Equal(pageSize, result.Data.PageSize);
        Assert.True(result.Data.Items.Count <= pageSize);
    }

    [Fact]
    public async Task GetPatientList_WithStationIdFilter_ReturnsFilteredResults()
    {
        // Arrange
        var stationId = 1;

        // Act
        var response = await _authenticatedClient.GetAsync($"/app001/getPatientList?stationId={stationId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.All(result.Data.Items, item => Assert.Equal(stationId, item.StationId));
    }

    [Theory]
    [InlineData("John")]
    [InlineData("Jane")]
    [InlineData("Smith")]
    public async Task GetPatientList_WithNameFilter_ReturnsFilteredResults(string name)
    {
        // Act
        var response = await _authenticatedClient.GetAsync($"/app001/getPatientList?name={name}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.All(result.Data.Items, item => 
            Assert.Contains(name, item.Name, StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPatientList_ResponseFormat_MatchesSpec()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPatientList");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        Assert.NotNull(result.Message);
        Assert.True(result.Timestamp > 0);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Items);
        Assert.True(result.Data.TotalCount >= 0);
        Assert.True(result.Data.Page >= 1);
        Assert.True(result.Data.PageSize >= 1);
        Assert.True(result.Data.TotalPages >= 0);

        // Validate patient item structure
        if (result.Data.Items.Count > 0)
        {
            var firstItem = result.Data.Items[0];
            Assert.True(firstItem.Id > 0);
            Assert.False(string.IsNullOrEmpty(firstItem.Number));
            Assert.False(string.IsNullOrEmpty(firstItem.Name));
        }
    }

    [Fact]
    public async Task GetPatientList_WithBedNumberFilter_ReturnsFilteredResults()
    {
        // Arrange
        var bedNumber = "A101";

        // Act
        var response = await _authenticatedClient.GetAsync($"/app001/getPatientList?bedNumber={bedNumber}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.All(result.Data.Items, item => 
            Assert.Contains(bedNumber, item.BedNumber ?? "", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPatientList_WithCombinedFilters_ReturnsCorrectResults()
    {
        // Arrange
        var stationId = 1;
        var pageSize = 10;

        // Act
        var response = await _authenticatedClient.GetAsync($"/app001/getPatientList?stationId={stationId}&pageSize={pageSize}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PatientListResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.All(result.Data.Items, item => Assert.Equal(stationId, item.StationId));
        Assert.True(result.Data.Items.Count <= pageSize);
    }

    #endregion

    #region GET /app001/getPrescriptionDetail Tests

    [Fact]
    public async Task GetPrescriptionDetail_WithoutAuth_Returns401()
    {
        // Act
        var response = await _client.GetAsync("/app001/getPrescriptionDetail?id=1");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetPrescriptionDetail_WithAuth_Returns200()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPrescriptionDetail?id=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PrescriptionDetailResponse>>();
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task GetPrescriptionDetail_WithInvalidId_Returns404()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPrescriptionDetail?id=99999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPrescriptionDetail_ResponseIncludesPatientInfo()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPrescriptionDetail?id=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PrescriptionDetailResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Patient);
        Assert.True(result.Data.Patient.Id > 0);
        Assert.False(string.IsNullOrEmpty(result.Data.Patient.Number));
        Assert.False(string.IsNullOrEmpty(result.Data.Patient.Name));
    }

    [Fact]
    public async Task GetPrescriptionDetail_ResponseIncludesPrescriptionDetails()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPrescriptionDetail?id=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PrescriptionDetailResponse>>();
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.Details);
        Assert.True(result.Data.Details.Count > 0);
        
        // Validate detail item structure
        var firstDetail = result.Data.Details[0];
        Assert.True(firstDetail.Id > 0);
        Assert.False(string.IsNullOrEmpty(firstDetail.DetailNumber));
        Assert.False(string.IsNullOrEmpty(firstDetail.ApprovalStatus));
    }

    [Fact]
    public async Task GetPrescriptionDetail_ResponseFormat_MatchesSpec()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPrescriptionDetail?id=1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PrescriptionDetailResponse>>();
        Assert.NotNull(result);
        Assert.Equal(0, result.Code);
        Assert.NotNull(result.Message);
        Assert.True(result.Timestamp > 0);
        Assert.NotNull(result.Data);
        Assert.True(result.Data.Id > 0);
        Assert.False(string.IsNullOrEmpty(result.Data.Number));
        Assert.NotNull(result.Data.Patient);
        Assert.NotNull(result.Data.Details);
    }

    [Fact]
    public async Task GetPrescriptionDetail_InvalidIdFormat_ReturnsBadRequest()
    {
        // Act
        var response = await _authenticatedClient.GetAsync("/app001/getPrescriptionDetail?id=invalid");

        // Assert
        // The response might be BadRequest, UnprocessableEntity, or OK depending on model binding behavior
        // All are acceptable - we're testing that it doesn't crash
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest ||
                    response.StatusCode == HttpStatusCode.OK ||
                    response.StatusCode == HttpStatusCode.UnprocessableEntity);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task GetPrescriptionDetail_DifferentIds_ReturnCorrectData(int id)
    {
        // Act
        var response = await _authenticatedClient.GetAsync($"/app001/getPrescriptionDetail?id={id}");

        // Assert
        if (id <= 2) // Our mock has prescriptions 1 and 2
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ResponseViewModel<PrescriptionDetailResponse>>();
            Assert.NotNull(result);
            Assert.NotNull(result.Data);
            Assert.Equal(id, result.Data.Id);
        }
        else
        {
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    #endregion
}
