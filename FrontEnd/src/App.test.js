import { render, screen, fireEvent } from '@testing-library/react';
import App from './App';

test('renders SearchJob brand and login button', () => {
  render(<App />);
  const logoElements = screen.getAllByText(/Search/i);
  expect(logoElements.length).toBeGreaterThan(0);

  const loginButton = screen.getByRole('button', { name: /Log in/i });
  expect(loginButton).toBeInTheDocument();
});

test('navigates to login page and displays login form', () => {
  render(<App />);
  const loginButton = screen.getByRole('button', { name: /Log in/i });
  fireEvent.click(loginButton);

  expect(screen.getByText(/Вхід у SearchJob/i)).toBeInTheDocument();
  expect(screen.getByLabelText(/^Електронна пошта$/i)).toBeInTheDocument();
  expect(screen.getByLabelText(/^Пароль$/i)).toBeInTheDocument();
});

