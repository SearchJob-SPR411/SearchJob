import loginService, { HARDCODED_ACCOUNTS } from './loginService';

describe('loginService (Hardcoded login tests)', () => {
  beforeEach(() => {
    loginService.logout();
  });

  test('has preconfigured hardcoded test accounts', () => {
    expect(HARDCODED_ACCOUNTS.length).toBeGreaterThan(0);
    expect(HARDCODED_ACCOUNTS[0].email).toBe('admin@searchjob.com');
  });

  test('successfully logs in with valid hardcoded credentials', async () => {
    const result = await loginService.login('admin@searchjob.com', 'password123');
    expect(result.success).toBe(true);
    expect(result.user).toBeDefined();
    expect(result.user.email).toBe('admin@searchjob.com');
    expect(loginService.getCurrentUser()?.email).toBe('admin@searchjob.com');
  });

  test('fails with invalid credentials', async () => {
    const result = await loginService.login('admin@searchjob.com', 'wrongpassword');
    expect(result.success).toBe(false);
    expect(result.message).toContain('Невірний email');
    expect(loginService.getCurrentUser()).toBeNull();
  });

  test('fails with empty credentials', async () => {
    const result = await loginService.login('', '');
    expect(result.success).toBe(false);
    expect(result.message).toContain('email та пароль');
  });

  test('logout clears the current user session', async () => {
    await loginService.login('demo@searchjob.com', 'demo');
    expect(loginService.getCurrentUser()).not.toBeNull();
    loginService.logout();
    expect(loginService.getCurrentUser()).toBeNull();
  });
});
