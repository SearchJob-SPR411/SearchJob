/**
 * Hardcoded Login Service for SearchJob (SPR-411 Issue #8).
 * Supports hardcoded credential verification and connects to backend API if available.
 */

export const HARDCODED_ACCOUNTS = [
  {
    email: "admin@searchjob.com",
    password: "password123",
    role: "Admin",
    user: {
      id: 1,
      firstName: "Admin",
      lastName: "SearchJob",
      email: "admin@searchjob.com",
      phoneNumber: "+380501234567",
    },
  },
  {
    email: "demo@searchjob.com",
    password: "demo",
    role: "Демо користувач",
    user: {
      id: 2,
      firstName: "Демо",
      lastName: "Користувач",
      email: "demo@searchjob.com",
      phoneNumber: "+380671112233",
    },
  },
  {
    email: "hr@searchjob.com",
    password: "hr123",
    role: "HR Менеджер",
    user: {
      id: 3,
      firstName: "Олена",
      lastName: "Коваль",
      email: "hr@searchjob.com",
      phoneNumber: "+380931234567",
    },
  },
];

const STORAGE_KEY = "searchjob_auth_user";
const TOKEN_KEY = "searchjob_auth_token";

export const loginService = {
  /**
   * Performs login with hardcoded authentication or backend API.
   * @param {string} email
   * @param {string} password
   * @returns {Promise<{success: boolean, message: string, user?: object, token?: string}>}
   */
  async login(email, password) {
    const trimmedEmail = (email || "").trim().toLowerCase();
    const trimmedPassword = password || "";

    if (!trimmedEmail || !trimmedPassword) {
      return {
        success: false,
        message: "Будь ласка, введіть email та пароль",
      };
    }

    // Try backend API first (if available and not in unit test environment)
    if (process.env.NODE_ENV !== "test") {
      try {
        const response = await fetch("https://localhost:7045/api/auth/login", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify({ email: trimmedEmail, password: trimmedPassword }),
        });

        if (response.ok) {
          const data = await response.json();
          if (data.isSuccess) {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(data.user));
            if (data.token) localStorage.setItem(TOKEN_KEY, data.token);
            return {
              success: true,
              message: data.message || "Успішний вхід",
              user: data.user,
              token: data.token,
            };
          }
        }
      } catch {
        // Backend not running, seamlessly fallback to hardcoded validation
      }
    }

    // Hardcoded authentication check
    const matched = HARDCODED_ACCOUNTS.find(
      (acc) =>
        acc.email.toLowerCase() === trimmedEmail &&
        acc.password === trimmedPassword
    );

    if (matched) {
      const token = `mock-token-${Date.now()}`;
      localStorage.setItem(STORAGE_KEY, JSON.stringify(matched.user));
      localStorage.setItem(TOKEN_KEY, token);

      return {
        success: true,
        message: `Ласкаво просимо, ${matched.user.firstName}!`,
        user: matched.user,
        token,
      };
    }

    return {
      success: false,
      message: "Невірний email або пароль",
    };
  },

  /**
   * Logs out the current user.
   */
  logout() {
    localStorage.removeItem(STORAGE_KEY);
    localStorage.removeItem(TOKEN_KEY);
  },

  /**
   * Returns current authenticated user or null.
   */
  getCurrentUser() {
    try {
      const userStr = localStorage.getItem(STORAGE_KEY);
      return userStr ? JSON.parse(userStr) : null;
    } catch {
      return null;
    }
  },

  /**
   * Returns list of test credentials.
   */
  getTestAccounts() {
    return HARDCODED_ACCOUNTS;
  },
};

export default loginService;
