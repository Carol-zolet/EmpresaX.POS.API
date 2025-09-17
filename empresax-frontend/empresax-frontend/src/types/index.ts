// Tipos para o sistema financeiro
export interface Estatisticas {
  sucesso: boolean;
  total: number;
  totalFormatado: string;
  pendentes: number;
  pendentesFormatado: string;
  pagas: number;
  pagasFormatado: string;
  vencidas: number;
  vencidasFormatado: string;
  quantidadeTotal: number;
  quantidadePendentes: number;
  quantidadeVencidas: number;
}

export interface ContaPagar {
  id: number;
  fornecedor: string;
  descricao: string;
  valor: number;
  vencimento: string;
  status: string;
  categoria: string;
  createdAt: string;
}

export interface Cliente {
  id: number;
  nome: string;
  email: string;
  telefone?: string;
  totalCompras: number;
  ultimaCompra: string;
  status: "Ativo" | "Inativo";
}

export interface Produto {
  id: string;
  nome: string;
  categoria: string;
  custo: number;
  venda: number;
  estoque: number;
  vendidos: number;
}

export interface ItemABC {
  nome: string;
  valor: number;
  percentual: number;
  percentualAcumulado: number;
  categoria: "A" | "B" | "C";
}

export interface DREData {
  receita_bruta: number;
  deducoes: number;
  receita_liquida: number;
  custos: number;
  lucro_bruto: number;
  despesas: number;
  lucro_liquido: number;
  margem_liquida: number;
}
