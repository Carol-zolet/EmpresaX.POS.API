import React from 'react';
import { useLoading } from '../context/LoadingContext';
import './LoadingOverlay.css';

const LoadingOverlay: React.FC = () => {
  const { loading } = useLoading();
  if (!loading) return null;
  return (
    <div className="loading-overlay">
      <div className="spinner" />
      <span>Processando...</span>
    </div>
  );
};

export default LoadingOverlay;
