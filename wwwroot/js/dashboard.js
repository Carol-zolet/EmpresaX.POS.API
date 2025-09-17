// API Base URL
const API_BASE = 'http://localhost:5245/api';

// Funções de navegação
function showDashboard() {
    setActiveMenu();
    loadDashboard();
}

function showContasPagar() {
    setActiveMenu();
    loadContasPagar();
}

function showFluxoCaixa() {
    setActiveMenu();
    loadFluxoCaixa();
}

function showConciliacao() {
    setActiveMenu();
    loadConciliacao();
}

function setActiveMenu() {
    document.querySelectorAll('.nav-link').forEach(link => {
        link.classList.remove('active');
    });
    
    if(event && event.target) {
        event.target.classList.add('active');
    }
}

// Função para fazer requisições à API
async function fetchAPI(endpoint) {
    try {
        console.log('Fazendo requisição para:', API_BASE + endpoint);
        const response = await fetch(API_BASE + endpoint);
        if (!response.ok) {
            console.warn('Erro na API:', response.status);
            return null;
        }
        const data = await response.json();
        console.log('Dados recebidos:', data);
        return data;
    } catch (error) {
        console.error('Erro na requisição:', error);
        return null;
    }
}

// Dashboard principal
async function loadDashboard() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <h1 class="mb-4">Dashboard Financeiro</h1>
        
        <div class="row mb-4">
            <div class="col-md-3">
                <div class="card card-metric">
                    <div class="card-body d-flex align-items-center">
                        <div class="metric-icon bg-success-gradient me-3">
                            <i class="fas fa-wallet"></i>
                        </div>
                        <div>
                            <h6 class="card-title mb-0">Saldo Atual</h6>
                            <h4 class="mb-0" id="saldo-atual">R$ 45.623,45</h4>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-3">
                <div class="card card-metric">
                    <div class="card-body d-flex align-items-center">
                        <div class="metric-icon bg-primary-gradient me-3">
                            <i class="fas fa-file-invoice"></i>
                        </div>
                        <div>
                            <h6 class="card-title mb-0">Contas Pendentes</h6>
                            <h4 class="mb-0" id="contas-pendentes">-</h4>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-3">
                <div class="card card-metric">
                    <div class="card-body d-flex align-items-center">
                        <div class="metric-icon bg-warning-gradient me-3">
                            <i class="fas fa-exclamation-triangle"></i>
                        </div>
                        <div>
                            <h6 class="card-title mb-0">Vencidas</h6>
                            <h4 class="mb-0" id="contas-vencidas">-</h4>
                        </div>
                    </div>
                </div>
            </div>
            
            <div class="col-md-3">
                <div class="card card-metric">
                    <div class="card-body d-flex align-items-center">
                        <div class="metric-icon bg-info-gradient me-3">
                            <i class="fas fa-university"></i>
                        </div>
                        <div>
                            <h6 class="card-title mb-0">Contas Bancárias</h6>
                            <h4 class="mb-0">3</h4>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">Fluxo de Caixa - Últimos 30 dias</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="fluxoChart" style="height: 300px;"></canvas>
                    </div>
                </div>
            </div>
            
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">Contas Vencendo</h5>
                    </div>
                    <div class="card-body" id="contas-vencendo">
                        Carregando...
                    </div>
                </div>
            </div>
        </div>
    `;
    
    // Carregar dados das contas
    loadContasData();
    
    // Criar gráfico
    createFluxoChart();
}

// Carregar dados das contas para o dashboard
async function loadContasData() {
    const contas = await fetchAPI('/v1/financeiro/contas-pagar');
    
    if (contas && contas.data) {
        const pendentes = contas.data.filter(c => c.status === 'Pendente').length;
        const vencidas = contas.data.filter(c => c.status === 'Vencida').length;
        
        document.getElementById('contas-pendentes').textContent = pendentes;
        document.getElementById('contas-vencidas').textContent = vencidas;
        
        // Mostrar contas vencendo
        const vencendo = contas.data
            .filter(c => c.status === 'Pendente')
            .slice(0, 3);
            
        const vencendoHtml = vencendo.map(conta => 
            '<div class="d-flex justify-content-between align-items-center mb-2">' +
            '<div><strong>' + conta.fornecedor + '</strong><br>' +
            '<small class="text-muted">' + conta.descricao + '</small></div>' +
            '<div class="text-end">' +
            '<strong>R$ ' + conta.valor + '</strong><br>' +
            '<small class="text-danger">' + conta.vencimento + '</small>' +
            '</div></div><hr>'
        ).join('');
        
        document.getElementById('contas-vencendo').innerHTML = 
            vencendoHtml || '<p class="text-muted">Nenhuma conta vencendo</p>';
    } else {
        document.getElementById('contas-pendentes').textContent = '0';
        document.getElementById('contas-vencidas').textContent = '0';
        document.getElementById('contas-vencendo').innerHTML = 
            '<p class="text-muted">Erro ao carregar dados</p>';
    }
}

// Criar gráfico do fluxo de caixa
function createFluxoChart() {
    const ctx = document.getElementById('fluxoChart');
    if (!ctx) return;
    
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['1 Dez', '5 Dez', '10 Dez', '15 Dez', '20 Dez', '25 Dez', '30 Dez'],
            datasets: [{
                label: 'Saldo',
                data: [45000, 47000, 44000, 46000, 45623, 48000, 47500],
                borderColor: '#007bff',
                backgroundColor: 'rgba(0, 123, 255, 0.1)',
                fill: true,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    beginAtZero: false,
                    ticks: {
                        callback: function(value) {
                            return 'R$ ' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

// Contas a Pagar
async function loadContasPagar() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h1>Contas a Pagar</h1>
            <button class="btn btn-primary" onclick="showNovaConta()">
                <i class="fas fa-plus me-2"></i>Nova Conta
            </button>
        </div>
        
        <div class="card">
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>Fornecedor</th>
                                <th>Descrição</th>
                                <th>Valor</th>
                                <th>Vencimento</th>
                                <th>Status</th>
                                <th>Ações</th>
                            </tr>
                        </thead>
                        <tbody id="contas-tbody">
                            <tr><td colspan="6" class="text-center">Carregando...</td></tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    `;
    
    const contas = await fetchAPI('/v1/financeiro/contas-pagar');
    if (contas && contas.data) {
        renderContasPagar(contas.data);
    } else {
        document.getElementById('contas-tbody').innerHTML = 
            '<tr><td colspan="6" class="text-center text-danger">Erro ao carregar dados</td></tr>';
    }
}

// Renderizar tabela de contas a pagar
function renderContasPagar(contas) {
    const tbody = document.getElementById('contas-tbody');
    
    if (!contas || contas.length === 0) {
        tbody.innerHTML = '<tr><td colspan="6" class="text-center text-muted">Nenhuma conta encontrada</td></tr>';
        return;
    }
    
    tbody.innerHTML = contas.map(conta => 
        '<tr>' +
        '<td><strong>' + conta.fornecedor + '</strong></td>' +
        '<td>' + conta.descricao + '</td>' +
        '<td><strong>R$ ' + conta.valor + '</strong></td>' +
        '<td>' + conta.vencimento + '</td>' +
        '<td><span class="badge ' + getStatusClass(conta.status) + '">' + conta.status + '</span></td>' +
        '<td>' +
        '<button class="btn btn-sm btn-outline-primary me-1" onclick="editarConta(' + conta.id + ')" title="Editar">' +
        '<i class="fas fa-edit"></i></button>' +
        '<button class="btn btn-sm btn-outline-danger" onclick="excluirConta(' + conta.id + ')" title="Excluir">' +
        '<i class="fas fa-trash"></i></button>' +
        '</td>' +
        '</tr>'
    ).join('');
}

// Fluxo de Caixa
async function loadFluxoCaixa() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <h1 class="mb-4">Fluxo de Caixa</h1>
        
        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card">
                    <div class="card-body text-center">
                        <h5>Posição Atual</h5>
                        <h2 class="text-success">R$ 45.623,45</h2>
                        <small class="text-muted">Atualizado em ${new Date().toLocaleString()}</small>
                    </div>
                </div>
            </div>
            
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">Últimas Movimentações</h5>
                    </div>
                    <div class="card-body">
                        <div class="d-flex justify-content-between align-items-center mb-3">
                            <div>
                                <strong>Pagamento Fornecedor ABC</strong><br>
                                <small class="text-muted">Material de escritório</small>
                            </div>
                            <div class="text-end">
                                <strong class="text-danger">- R$ 1.250,00</strong><br>
                                <small class="text-muted">Hoje</small>
                            </div>
                        </div>
                        <hr>
                        <div class="d-flex justify-content-between align-items-center mb-3">
                            <div>
                                <strong>Recebimento Cliente XYZ</strong><br>
                                <small class="text-muted">Pagamento serviços</small>
                            </div>
                            <div class="text-end">
                                <strong class="text-success">+ R$ 3.500,00</strong><br>
                                <small class="text-muted">Ontem</small>
                            </div>
                        </div>
                        <hr>
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <strong>Pagamento Energia Elétrica</strong><br>
                                <small class="text-muted">Conta de luz</small>
                            </div>
                            <div class="text-end">
                                <strong class="text-danger">- R$ 850,30</strong><br>
                                <small class="text-muted">2 dias atrás</small>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row">
            <div class="col-12">
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">Gráfico de Movimentações</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="fluxoDetailChart" style="height: 300px;"></canvas>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    // Criar gráfico detalhado do fluxo
    setTimeout(createFluxoDetailChart, 100);
}

// Criar gráfico detalhado do fluxo de caixa
function createFluxoDetailChart() {
    const ctx = document.getElementById('fluxoDetailChart');
    if (!ctx) return;
    
    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Entradas', 'Saídas', 'Saldo'],
            datasets: [{
                label: 'Valores (R$)',
                data: [78500, -32876, 45624],
                backgroundColor: ['#28a745', '#dc3545', '#007bff'],
                borderColor: ['#28a745', '#dc3545', '#007bff'],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
                y: {
                    ticks: {
                        callback: function(value) {
                            return 'R$ ' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });
}

// Conciliação Bancária
async function loadConciliacao() {
    const content = document.getElementById('content');
    content.innerHTML = `
        <h1 class="mb-4">Conciliação Bancária</h1>
        
        <div class="card">
            <div class="card-header">
                <h5 class="mb-0">Contas Bancárias</h5>
            </div>
            <div class="card-body">
                <div class="row mb-4">
                    <div class="col-md-4">
                        <div class="card border-primary">
                            <div class="card-body">
                                <h6><strong>Banco do Brasil</strong></h6>
                                <p class="mb-1">Ag: 1234-5 | Conta: 12345-6</p>
                                <span class="badge bg-info mb-2">Conta Corrente</span>
                                <h4 class="text-success">R$ 25.420,80</h4>
                                <button class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-sync me-1"></i>Conciliar
                                </button>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-md-4">
                        <div class="card border-warning">
                            <div class="card-body">
                                <h6><strong>Itaú</strong></h6>
                                <p class="mb-1">Ag: 4567 | Conta: 98765-4</p>
                                <span class="badge bg-info mb-2">Conta Corrente</span>
                                <h4 class="text-success">R$ 18.750,65</h4>
                                <button class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-sync me-1"></i>Conciliar
                                </button>
                            </div>
                        </div>
                    </div>
                    
                    <div class="col-md-4">
                        <div class="card border-success">
                            <div class="card-body">
                                <h6><strong>Santander</strong></h6>
                                <p class="mb-1">Ag: 8901 | Conta: 54321-9</p>
                                <span class="badge bg-info mb-2">Conta Poupança</span>
                                <h4 class="text-success">R$ 12.500,00</h4>
                                <button class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-sync me-1"></i>Conciliar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-12">
                        <div class="card bg-light">
                            <div class="card-body text-center">
                                <h5 class="mb-3">Resumo Geral</h5>
                                <div class="row">
                                    <div class="col-md-4">
                                        <h6>Total em Bancos</h6>
                                        <h4 class="text-primary">R$ 56.671,45</h4>
                                    </div>
                                    <div class="col-md-4">
                                        <h6>Saldo Sistema</h6>
                                        <h4 class="text-success">R$ 45.623,45</h4>
                                    </div>
                                    <div class="col-md-4">
                                        <h6>Diferença</h6>
                                        <h4 class="text-warning">R$ 11.048,00</h4>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

// Funções auxiliares
function getStatusClass(status) {
    switch(status) {
        case 'Paga': return 'bg-success text-white';
        case 'Pendente': return 'bg-warning text-dark';
        case 'Vencida': return 'bg-danger text-white';
        default: return 'bg-secondary text-white';
    }
}

function showNovaConta() {
    alert('Funcionalidade de nova conta será implementada');
}

function editarConta(id) {
    alert('Editar conta ID: ' + id + '\nFuncionalidade em desenvolvimento');
}

function excluirConta(id) {
    if (confirm('Tem certeza que deseja excluir esta conta?')) {
        alert('Conta ID: ' + id + ' excluída!\nFuncionalidade será implementada');
    }
}

// Inicializar aplicação
document.addEventListener('DOMContentLoaded', function() {
    console.log('Sistema Financeiro EmpresaX carregado!');
    loadDashboard();
});