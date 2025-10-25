import { useState } from 'react';
import { 
  HomeIcon, 
  CurrencyDollarIcon, 
  UserGroupIcon, 
  CubeIcon,
  DocumentChartBarIcon,
  ChartBarIcon,
  Bars3Icon,
  XMarkIcon,
  ServerStackIcon
} from '@heroicons/react/24/outline';

interface LayoutProps {
  children: React.ReactNode;
  currentPage: string;
}

export default function Layout({ children, currentPage }: LayoutProps) {
  const [sidebarOpen, setSidebarOpen] = useState(false);

  const navigation = [
    { name: 'Dashboard', href: '/', icon: HomeIcon },
    { name: 'Contas a Pagar', href: '/contas-pagar', icon: CurrencyDollarIcon },
    { name: 'Clientes', href: '/clientes', icon: UserGroupIcon },
    { name: 'Produtos', href: '/produtos', icon: CubeIcon },
    { name: 'DRE/Caixa', href: '/caixa', icon: DocumentChartBarIcon },
    { name: 'Curva ABC', href: '/curva-abc', icon: ChartBarIcon },
    { name: 'Diagnóstico Kafka', href: '/diagnostics-kafka', icon: ServerStackIcon },
  ];

  return (
    <div className="layout-container">
      {/* Mobile Overlay */}
      {sidebarOpen && (
        <div className="fixed inset-0 z-40 lg:hidden">
          <div className="fixed inset-0 bg-gray-600 bg-opacity-75" onClick={() => setSidebarOpen(false)} />
        </div>
      )}

      {/* Sidebar */}
      <div className={`sidebar ${sidebarOpen ? 'open' : ''}`}>
        <div className="sidebar-header">
          <h1 className="sidebar-title">EmpresaX POS</h1>
        </div>
        <nav>
          <ul className="nav-list">
            {navigation.map((item) => (
              <li key={item.name} className="nav-item">
                <a
                  href={item.href}
                  className={`nav-link ${currentPage === item.href ? 'active' : ''}`}
                >
                  <item.icon className="nav-icon" />
                  {item.name}
                </a>
              </li>
            ))}
          </ul>
        </nav>
      </div>

      {/* Main Content */}
      <div className="flex-1">
        {/* Mobile Header */}
        <div className="mobile-header">
          <button onClick={() => setSidebarOpen(true)}>
            <Bars3Icon className="h-6 w-6" />
          </button>
          <h1 className="font-bold">EmpresaX</h1>
          <div></div>
        </div>

        <main className="main-content">
          {children}
        </main>
      </div>
    </div>
  );
}
