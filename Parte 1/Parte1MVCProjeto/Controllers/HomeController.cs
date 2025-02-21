using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Parte1MVCProjeto.Models;

namespace Parte1MVCProjeto.Controllers;

public class HomeController : Controller
{
    private readonly long _tamanhoMaximoArquivo = 50 * 1024 * 1024;
    private readonly string _extensaoArquivoPermitido = ".csv";
    private readonly string _caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

    public IActionResult Index()
    {
        return View();
    }

    // Parte 1
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            ViewBag.Mensagem = "Nenhum arquivo selecionado.";
            return View("Index");
        }

        if (arquivo.Length > _tamanhoMaximoArquivo)
        {
            ViewBag.Mensagem = "O arquivo excede o limite de 50MB.";
            return View("Index");
        }

        var extensao = Path.GetExtension(arquivo.FileName).ToLower();
        
        if (_extensaoArquivoPermitido != extensao)
        {
            ViewBag.Mensagem = "Apenas arquivos .csv são permitidos.";
            return View("Index");
        }
        
        if (!Directory.Exists(_caminhoArquivo))
        {
            Directory.CreateDirectory(_caminhoArquivo);
        }

        var filePath = Path.Combine(_caminhoArquivo, Path.GetFileName(arquivo.FileName));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await arquivo.CopyToAsync(stream);
        }
        
        ViewBag.Mensagem = $"Arquivo '{arquivo.FileName}' enviado com sucesso!";
        
        return View("Index");
    }
    
    // Parte 2
    [HttpGet]
    public IActionResult SelecionarArquivos()
    {
        var arquivos = Directory.GetFiles(_caminhoArquivo, "*.csv")
            .Select(Path.GetFileName)
            .ToList();

        return View(arquivos); 
    }
    
    [HttpPost]
    public IActionResult MostrarColunas(string arquivo1, string arquivo2)
    {
        var caminhoArquivo1 = Path.Combine(_caminhoArquivo, arquivo1);
        var caminhoArquivo2 = Path.Combine(_caminhoArquivo, arquivo2);
        
        var colunasArquivo1 = ObterColunasCsv(caminhoArquivo1);
        var colunasArquivo2 = ObterColunasCsv(caminhoArquivo2);
        
        var model = new ArquivosViewModel
        {
            NomeArquivo1 = arquivo1,
            ColunasArquivo1 = colunasArquivo1,
            NomeArquivo2 = arquivo2,
            ColunasArquivo2 = colunasArquivo2
        };

        return View(model);
    }
    
    private List<string> ObterColunasCsv(string caminhoArquivo)
    {
        using (var reader = new StreamReader(caminhoArquivo))
        {
            var primeiraLinha = reader.ReadLine();
            if (!string.IsNullOrEmpty(primeiraLinha))
            {
                return primeiraLinha.Split(',').Select(c => c.Trim('"')).ToList();
            }
        }

        return new List<string>();
    }
    public class ArquivosViewModel
    {
        public string NomeArquivo1 { get; set; }
        public List<string> ColunasArquivo1 { get; set; }
        public string NomeArquivo2 { get; set; }
        public List<string> ColunasArquivo2 { get; set; }
    }
}