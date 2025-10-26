import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';

const Login: React.FC = () => {
  const { setToken } = useAuth();
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [erro, setErro] = useState<string | null>(null);

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setErro(null);
    try {
      const resp = await fetch('/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, senha })
      });
      if (!resp.ok) {
        setErro('Usuário ou senha inválidos');
        return;
      }
      const data = await resp.json();
      setToken(data.token);
    } catch {
      setErro('Erro de rede');
    }
  };

  return (
    <form onSubmit={handleLogin} style={{ maxWidth: 320, margin: '2rem auto' }}>
      <h2>Login</h2>
      <label>Email<br />
        <input type="email" value={email} onChange={e => setEmail(e.target.value)} required />
      </label>
      <br />
      <label>Senha<br />
        <input type="password" value={senha} onChange={e => setSenha(e.target.value)} required />
      </label>
      <br />
      <button type="submit">Entrar</button>
      {erro && <div role="alert" style={{ color: 'red' }}>{erro}</div>}
    </form>
  );
};

export default Login;
