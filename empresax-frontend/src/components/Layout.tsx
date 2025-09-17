import { useState } from 'react';
import { 
  HomeIcon, 
  CurrencyDollarIcon, 
  UserGroupIcon, 
  CubeIcon,
  DocumentChartBarIcon,
  ChartBarIcon,
  Bars3Icon,
  XMarkIcon
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
  ];

  return (
    <div className="min-h-screen bg-gray-50 flex">
      {sidebarOpen && (
        <div className="fixed inset-0 z-40 lg:hidden">
          <div className="fixed inset-0 bg-gray-600 bg-opacity-75" onClick={() => setSidebarOpen(false)} />
          <div className="fixed inset-y-0 left-0 flex w-64 flex-col bg-white shadow-xl">
            <div className="flex h-16 shrink-0 items-center justify-between px-4">
              <h1 className="text-xl font-bold text-gray-900">EmpresaX</h1>
              <button onClick={() => setSidebarOpen(false)}>
                <XMarkIcon className="h-6 w-6" />
              </button>
            </div>
            <nav className="flex-1 space-y-1 px-2 py-4">
              {navigation.map((item) => (
                <a
                  key={item.name}
                  href={item.href}
                  className={`group flex items-center px-2 py-2 text-sm font-medium rounded-md ${
                    currentPage === item.href
                      ? 'bg-blue-100 text-blue-700'
                      : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900'
                  }`}
                >
                  <item.icon className="mr-3 h-6 w-6" />
                  {item.name}
                </a>
              ))}
            </nav>
          </div>
        </div>
      )}

      <div className="hidden lg:fixed lg:inset-y-0 lg:flex lg:w-64 lg:flex-col">
        <div className="flex min-h-0 flex-1 flex-col bg-white border-r border-gray-200">
          <div className="flex h-16 shrink-0 items-center px-4 border-b border-gray-200">
            <h1 className="text-xl font-bold text-gray-900">EmpresaX POS</h1>
          </div>
          <div className="flex flex-1 flex-col overflow-y-auto pt-5 pb-4">
            <nav className="flex-1 space-y-1 px-2">
              {navigation.map((item) => (
                <a
                  key={item.name}
                  href={item.href}
                  className={`group flex items-center px-2 py-2 text-sm font-medium rounded-md ${
                    currentPage === item.href
                      ? 'bg-blue-100 text-blue-700'
                      : 'text-gray-600 hover:bg-gray-50 hover:text-gray-900'
                  }`}
                >
                  <item.icon className="mr-3 h-6 w-6" />
                  {item.name}
                </a>
              ))}
            </nav>
          </div>
        </div>
      </div>

      <div className="flex flex-1 flex-col lg:pl-64">
        <div className="sticky top-0 z-10 flex h-16 shrink-0 items-center gap-x-4 border-b border-gray-200 bg-white px-4 shadow-sm lg:hidden">
          <button
            type="button"
            className="text-gray-700"
            onClick={() => setSidebarOpen(true)}
          >
            <Bars3Icon className="h-6 w-6" />
          </button>
          <h1 className="text-xl font-bold text-gray-900">EmpresaX</h1>
        </div>

        <main className="flex-1">
          {children}
        </main>
      </div>
    </div>
  );
}
