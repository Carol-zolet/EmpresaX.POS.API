
import { useState, useEffect } from 'react';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import KafkaDiagnostics from './pages/KafkaDiagnostics';
import ContasPagar from './pages/ContasPagar';
import Clientes from './pages/Clientes';
import Produtos from './pages/Produtos';
import Caixa from './pages/Caixa';
import CurvaABC from './pages/CurvaABC';
import { AuthProvider, useAuth } from './context/AuthContext';
import { LoadingProvider } from './context/LoadingContext';
import LoadingOverlay from './components/LoadingOverlay';
import ImportacaoPlanilha from './components/ImportacaoPlanilha';
import Login from './components/Login';

function App() {
  const [currentPage, setCurrentPage] = useState('/');

  const renderPage = () => {
    switch (currentPage) {
      case '/contas-pagar':
        return <ContasPagar />;
      case '/clientes':
        return <Clientes />;
      case '/produtos':
        return <Produtos />;
      case '/caixa':
        return <Caixa />;
      case '/curva-abc':
        return <CurvaABC />;
      case '/diagnostics-kafka':
        return <KafkaDiagnostics />;
      case '/importacao-planilha':
        return <ImportacaoPlanilha />;
      default:
        return <Dashboard />;
    }
  };

  useEffect(() => {
    const handleClick = (e: Event) => {
      const target = e.target as HTMLAnchorElement;
      if (target.tagName === 'A' && target.href && target.href.startsWith(window.location.origin)) {
        e.preventDefault();
        const url = new URL(target.href);
        setCurrentPage(url.pathname);
      }
    };

    document.addEventListener('click', handleClick);
    return () => document.removeEventListener('click', handleClick);
  }, []);

  // Wrapper para decidir entre login e app
  const Main = () => {
    const { token } = useAuth();
    if (!token) return <Login />;
    return (
      <LoadingProvider>
        <LoadingOverlay />
        <Layout currentPage={currentPage}>
          {renderPage()}
        </Layout>
      </LoadingProvider>
    );
  };
  return (
    <AuthProvider>
      <Main />
    </AuthProvider>
  );
}

export default App;
