import { useState } from "react";
import loginService, { HARDCODED_ACCOUNTS } from "../services/loginService";
import "./Login.css";

export default function Login({ onNavigate, onLoginSuccess }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [status, setStatus] = useState(null); // { type: 'success' | 'error', message: string }

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsLoading(true);
    setStatus(null);

    const result = await loginService.login(email, password);

    setIsLoading(false);
    if (result.success) {
      setStatus({ type: "success", message: result.message });
      if (onLoginSuccess) {
        onLoginSuccess(result.user);
      }
    } else {
      setStatus({ type: "error", message: result.message });
    }
  };

  const handleQuickFill = (account) => {
    setEmail(account.email);
    setPassword(account.password);
    setStatus(null);
  };

  return (
    <div className="login-page">
      {/* Header */}
      <header className="header">
        <div className="header-inner">
          <div
            className="logo logo-clickable"
            onClick={() => onNavigate && onNavigate("home")}
            role="button"
            tabIndex={0}
          >
            Search<span className="logo-accent">Job</span>
          </div>
          <nav className="nav">
            <button
              className="btn btn-outline"
              onClick={() => onNavigate && onNavigate("home")}
            >
              ← На головну
            </button>
          </nav>
        </div>
      </header>

      {/* Main Login Container */}
      <main className="login-main">
        <div className="login-card">
          <div className="login-header">
            <h2>Вхід у SearchJob</h2>
            <p>Увійдіть, щоб переглядати та відгукуватися на вакансії</p>
          </div>

          {/* Quick Fill / Hardcoded Accounts hint */}
          <div className="quick-fill-box">
            <span className="quick-fill-label">Хардкод-акаунти для тесту:</span>
            <div className="quick-fill-buttons">
              {HARDCODED_ACCOUNTS.map((acc, index) => (
                <button
                  key={index}
                  type="button"
                  className="quick-chip"
                  onClick={() => handleQuickFill(acc)}
                  title={`Email: ${acc.email}\nПароль: ${acc.password}`}
                >
                  {acc.role} ({acc.email.split("@")[0]})
                </button>
              ))}
            </div>
          </div>

          {/* Feedback message */}
          {status && (
            <div
              className={`login-alert ${
                status.type === "success" ? "alert-success" : "alert-error"
              }`}
            >
              {status.type === "success" ? "✓ " : "⚠ "}
              {status.message}
            </div>
          )}

          {/* Form */}
          <form className="login-form" onSubmit={handleSubmit}>
            <div className="form-group">
              <label htmlFor="email">Електронна пошта</label>
              <input
                id="email"
                type="email"
                required
                placeholder="name@example.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="email"
              />
            </div>

            <div className="form-group">
              <div className="label-row">
                <label htmlFor="password">Пароль</label>
                <a
                  href="#!"
                  className="forgot-link"
                  onClick={(e) => {
                    e.preventDefault();
                    alert("Функціонал відновлення паролю в розробці");
                  }}
                >
                  Забули пароль?
                </a>
              </div>
              <div className="password-input-wrapper">
                <input
                  id="password"
                  type={showPassword ? "text" : "password"}
                  required
                  placeholder="••••••••"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  autoComplete="current-password"
                />
                <button
                  type="button"
                  className="toggle-password-btn"
                  onClick={() => setShowPassword(!showPassword)}
                  aria-label="Показати/приховати пароль"
                >
                  {showPassword ? "🙈" : "👁️"}
                </button>
              </div>
            </div>

            <div className="form-checkbox">
              <label>
                <input
                  type="checkbox"
                  checked={rememberMe}
                  onChange={(e) => setRememberMe(e.target.checked)}
                />
                <span>Запам'ятати мене на цьому пристрої</span>
              </label>
            </div>

            <button
              type="submit"
              className="btn btn-primary btn-submit"
              disabled={isLoading}
            >
              {isLoading ? "Перевірка..." : "Увійти"}
            </button>
          </form>

          {/* Card footer */}
          <div className="login-card-footer">
            <span>Немає облікового запису? </span>
            <a
              href="#!"
              onClick={(e) => {
                e.preventDefault();
                alert("Сторінка реєстрації в розробці");
              }}
            >
              Зареєструватися
            </a>
          </div>
        </div>
      </main>

      {/* Footer */}
      <footer className="footer">
        <div className="footer-inner">© 2026 SearchJob. All rights reserved.</div>
      </footer>
    </div>
  );
}
