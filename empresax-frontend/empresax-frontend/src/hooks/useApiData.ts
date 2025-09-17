import { useState, useEffect } from 'react';

interface UseApiDataOptions<T> {
  url: string;
  fallbackData?: T;
  refreshInterval?: number;
}

export function useApiData<T>({ url, fallbackData, refreshInterval }: UseApiDataOptions<T>) {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isOffline, setIsOffline] = useState(false);

  const fetchData = async () => {
    try {
      const response = await fetch(url);
      if (!response.ok) throw new Error(`HTTP ${response.status}`);
      
      const result = await response.json();
      setData(result);
      setError(null);
      setIsOffline(false);
    } catch (err) {
      console.warn(`API offline: ${url}`, err);
      setError(err instanceof Error ? err.message : 'Erro desconhecido');
      setIsOffline(true);
      
      if (fallbackData) {
        setData(fallbackData);
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
    
    if (refreshInterval && refreshInterval > 0) {
      const interval = setInterval(fetchData, refreshInterval);
      return () => clearInterval(interval);
    }
  }, [url, refreshInterval]);

  return { data, loading, error, isOffline, refetch: fetchData };
}
