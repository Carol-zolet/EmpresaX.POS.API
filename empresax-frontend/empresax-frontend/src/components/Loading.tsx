interface LoadingProps {
  message?: string;
}

export default function Loading({ message = "Carregando..." }: LoadingProps) {
  return (
    <div className="loading-container">
      <div className="loading-spinner"></div>
      <p style={{ marginTop: '1rem', color: '#4a5568' }}>{message}</p>
    </div>
  );
}
