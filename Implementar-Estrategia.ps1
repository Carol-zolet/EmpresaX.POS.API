# ===================================================================
# Script para Implementar o Roteiro Estratégico do Projeto
# 1. Cria o workflow de CI/CD do GitHub Actions.
# 2. Cria um arquivo de teste boilerplate para um novo controller.
# ===================================================================

$ErrorActionPreference = "Stop"
Write-Host "Iniciando a implementação da estratégia de testes e CI/CD..." -ForegroundColor Cyan

# --- FASE 3: Automação do Pipeline de CI/CD ---

$workflowDir = ".\.github\workflows"
$workflowFile = "$workflowDir\dotnet.yml"

Write-Host "`n--- Fase 3: Configurando o Workflow de CI/CD ---" -ForegroundColor Yellow

if (Test-Path $workflowFile) {
    Write-Host "O arquivo de workflow '$workflowFile' já existe." -ForegroundColor Green
} else {
    Write-Host "Criando o arquivo de workflow do GitHub Actions..."
    
    # Garante que a estrutura de pastas exista
    New-Item -ItemType Directory -Path $workflowDir -Force | Out-Null
    
    # Conteúdo do arquivo dotnet.yml
    $yamlContent = @"
# .github/workflows/dotnet.yml
name: .NET Build and Test

on:
  push:
    branches: [ "main", "develop" ]
  pull_request:
    branches: [ "main", "develop" ]

jobs:
  build:
    runs-on: ubuntu-latest

    steps:
    - name: Checkout repository
      uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore

    - name: Test
      run: dotnet test --no-build --verbosity normal
"@
    
    # Cria o arquivo
    Set-Content -Path $workflowFile -Value $yamlContent
    Write-Host "Arquivo '$workflowFile' criado com sucesso!" -ForegroundColor Green
}


# --- FASE 2: Criação de Novos Arquivos de Teste ---

Write-Host "`n--- Fase 2: Criar novo arquivo de teste (Scaffolding) ---" -ForegroundColor Yellow
$controllerName = Read-Host -Prompt "Digite o nome do Controller para o qual você quer criar um teste (ex: Produtos). Deixe em branco para pular"

if (-not [string]::IsNullOrWhiteSpace($controllerName)) {
    # Capitaliza o nome (ex: produtos -> Produtos)
    $controllerName = (Get-Culture).TextInfo.ToTitleCase($controllerName.ToLower())
    
    $testProjectDir = ".\EmpresaX.POS.API.Tests"
    $testFileDir = "$testProjectDir\Controllers"
    $testFileName = "$testFileDir\${controllerName}ControllerTests.cs"

    if (Test-Path $testFileName) {
        Write-Host "O arquivo de teste '$testFileName' já existe." -ForegroundColor Green
    } else {
        Write-Host "Criando arquivo de teste para '${controllerName}Controller'..."
        
        # Garante que a pasta Controllers exista
        New-Item -ItemType Directory -Path $testFileDir -Force | Out-Null
        
        # Template do arquivo de teste
        $csharpTemplate = @"
using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Net;
// TODO: Adicione os 'usings' para seus DTOs e Serviços
// using EmpresaX.POS.API.Services;
// using EmpresaX.POS.API.DTOs;
using EmpresaX.POS.API.Controllers;

namespace EmpresaX.POS.API.Tests.Controllers
{
    public class {CONTROLLER_NAME}ControllerTests
    {
        private readonly Mock<I{CONTROLLER_NAME}Service> _serviceMock;
        private readonly {CONTROLLER_NAME}Controller _controller;

        public {CONTROLLER_NAME}ControllerTests()
        {
            _serviceMock = new Mock<I{CONTROLLER_NAME}Service>();
            _controller = new {CONTROLLER_NAME}Controller(_serviceMock.Object);
        }

        [Fact]
        public void TesteDeExemplo_DeveRetornarSucesso()
        {
            // Arrange
            // Configure seu mock aqui. Ex:
            // _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<{CONTROLLER_NAME}Dto>());

            // Act
            // var resultado = await _controller.GetAll();

            // Assert
            // resultado.Should().NotBeNull();
            Assert.True(true); // Placeholder
        }
    }
}
"@
        # Substitui o placeholder pelo nome do controller
        $csharpContent = $csharpTemplate -replace "{CONTROLLER_NAME}", $controllerName
        
        # Cria o arquivo
        Set-Content -Path $testFileName -Value $csharpContent
        Write-Host "Arquivo '$testFileName' criado com sucesso!" -ForegroundColor Green
        Write-Host "Lembre-se de criar a interface 'I${controllerName}Service' e ajustar os usings." -ForegroundColor Cyan
    }
} else {
    Write-Host "Criação de arquivo de teste pulada."
}

Write-Host "`nEstratégia implementada com sucesso!" -ForegroundColor Green