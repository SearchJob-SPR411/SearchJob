import { useState, useEffect } from 'react';
import './App.css';
import Home from './components/Home';
import Login from './components/Login';
import loginService from './services/loginService';

function App() {
  const [currentPage, setCurrentPage] = useState(() => {
    return window.location.hash === '#login' ? 'login' : 'home';
  });
  const [currentUser, setCurrentUser] = useState(() => loginService.getCurrentUser());

  useEffect(() => {
    const handleHashChange = () => {
      if (window.location.hash === '#login') {
        setCurrentPage('login');
      } else {
        setCurrentPage('home');
      }
    };
    window.addEventListener('hashchange', handleHashChange);
    return () => window.removeEventListener('hashchange', handleHashChange);
  }, []);

  const navigate = (page) => {
    setCurrentPage(page);
    window.location.hash = page === 'login' ? 'login' : '';
  };

  const handleLoginSuccess = (user) => {
    setCurrentUser(user);
    setTimeout(() => {
      navigate('home');
    }, 1000);
  };

  const handleLogout = () => {
    loginService.logout();
    setCurrentUser(null);
  };

  if (currentPage === 'login') {
    return (
      <Login
        onNavigate={navigate}
        onLoginSuccess={handleLoginSuccess}
      />
    );
  }

  return (
    <Home
      onNavigate={navigate}
      currentUser={currentUser}
      onLogout={handleLogout}
    />
  );
}

export default App;
