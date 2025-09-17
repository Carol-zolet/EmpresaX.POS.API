export const API_CONFIG = {
  baseURL: 'http://localhost:5245/api/v1',
  timeout: 10000,
  endpoints: {
    financeiro: {
      estatisticas: '/financeiro/contas-pagar/estatisticas',
      contasPagar: '/financeiro/contas-pagar',
      importarExcel: '/financeiro/contas-pagar/importar-excel',
      template: '/financeiro/contas-pagar/template'
    },
    clientes: '/clientes',
    produtos: '/produtos'
  }
};

export const COLORS = {
  primary: '#3182ce',
  success: '#38a169',
  warning: '#d69e2e',
  error: '#e53e3e',
  purple: '#805ad5'
};
