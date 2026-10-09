import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Container,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  Typography,
  Alert,
  Paper,
  CircularProgress,
  Stack,
} from "@mui/material";
import { useNavigate, useSearchParams } from "react-router-dom";
import { api } from "../../api";

const EMPLOYMENT_TYPES = ["FullTime", "PartTime", "Contract", "Internship"];
const WORK_FORMATS = ["Office", "Remote", "Hybrid"];
const STATUSES = ["Active", "Draft", "Paused", "Closed"];

export default function CreateVacancy() {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const initialCompanyId = searchParams.get("companyId") || "";

  const [companies, setCompanies] = useState([]);
  const [loadingCompanies, setLoadingCompanies] = useState(true);

  const [formData, setFormData] = useState({
    title: "",
    companyId: initialCompanyId,
    location: "",
    employmentType: "FullTime",
    workFormat: "Office",
    salaryMin: "",
    salaryMax: "",
    status: "Active",
    description: "",
  });

  const [submitting, setSubmitting] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  useEffect(() => {
    const fetchCompanies = async () => {
      try {
        const res = await api.get("Companies");
        setCompanies(res.data || []);
        if (initialCompanyId) {
          setFormData((prev) => ({ ...prev, companyId: initialCompanyId }));
        } else if (res.data && res.data.length > 0) {
          setFormData((prev) => ({ ...prev, companyId: prev.companyId || res.data[0].id }));
        }
      } catch (err) {
        console.error("Failed to load companies:", err);
      } finally {
        setLoadingCompanies(false);
      }
    };

    fetchCompanies();
  }, [initialCompanyId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMessage("");
    setSuccessMessage("");

    if (!formData.title.trim()) {
      setErrorMessage("Будь ласка, вкажіть назву вакансії.");
      return;
    }
    if (!formData.companyId) {
      setErrorMessage("Будь ласка, оберіть компанію.");
      return;
    }
    if (!formData.location.trim()) {
      setErrorMessage("Будь ласка, вкажіть локацію.");
      return;
    }
    if (!formData.description.trim()) {
      setErrorMessage("Будь ласка, додайте опис вакансії.");
      return;
    }

    setSubmitting(true);

    const payload = {
      title: formData.title.trim(),
      companyId: Number(formData.companyId),
      location: formData.location.trim(),
      employmentType: formData.employmentType,
      workFormat: formData.workFormat,
      salaryMin: formData.salaryMin !== "" ? Number(formData.salaryMin) : null,
      salaryMax: formData.salaryMax !== "" ? Number(formData.salaryMax) : null,
      status: formData.status,
      description: formData.description.trim(),
    };

    try {
      const response = await api.post("Vacancies", payload);
      setSuccessMessage("Вакансію успішно створено!");
      setTimeout(() => {
        if (response.data?.id) {
          navigate(`/vacancies/${response.data.id}`);
        } else {
          navigate("/");
        }
      }, 1000);
    } catch (err) {
      console.error("Failed to create vacancy:", err);
      if (err.response?.status === 401 || err.response?.status === 403) {
        setErrorMessage(
          "Для створення вакансії необхідна авторизація з роллю Recruiter. Будь ласка, увійдіть в акаунт роботодавця."
        );
      } else if (err.response?.data?.message) {
        setErrorMessage(err.response.data.message);
      } else {
        setErrorMessage("Помилка під час створення вакансії. Спробуйте пізніше.");
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ py: 5 }}>
      <Paper sx={{ p: 4, borderRadius: 2 }}>
        <Typography variant="h4" gutterBottom fontWeight="bold">
          Створити нову вакансію
        </Typography>
        <Typography color="text.secondary" sx={{ mb: 4 }}>
          Заповніть деталі вакансії для пошуку кандидатів.
        </Typography>

        {errorMessage && (
          <Alert severity="error" sx={{ mb: 3 }}>
            {errorMessage}
          </Alert>
        )}

        {successMessage && (
          <Alert severity="success" sx={{ mb: 3 }}>
            {successMessage}
          </Alert>
        )}

        <Box component="form" onSubmit={handleSubmit} noValidate>
          <Stack spacing={3}>
            <TextField
              label="Назва вакансії *"
              name="title"
              value={formData.title}
              onChange={handleChange}
              placeholder="наприклад: Senior .NET Developer"
              fullWidth
              required
            />

            <FormControl fullWidth required>
              <InputLabel id="company-label">Компанія *</InputLabel>
              <Select
                labelId="company-label"
                name="companyId"
                value={formData.companyId}
                label="Компанія *"
                onChange={handleChange}
                disabled={loadingCompanies}
              >
                {loadingCompanies ? (
                  <MenuItem value="">
                    <em>Завантаження компаній...</em>
                  </MenuItem>
                ) : companies.length === 0 ? (
                  <MenuItem value="">
                    <em>Немає доступних компаній</em>
                  </MenuItem>
                ) : (
                  companies.map((c) => (
                    <MenuItem key={c.id} value={c.id}>
                      {c.name}
                    </MenuItem>
                  ))
                )}
              </Select>
            </FormControl>

            <TextField
              label="Локація *"
              name="location"
              value={formData.location}
              onChange={handleChange}
              placeholder="наприклад: Київ, Україна або Віддалено"
              fullWidth
              required
            />

            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
              <FormControl fullWidth>
                <InputLabel id="emp-type-label">Тип зайнятості</InputLabel>
                <Select
                  labelId="emp-type-label"
                  name="employmentType"
                  value={formData.employmentType}
                  label="Тип зайнятості"
                  onChange={handleChange}
                >
                  {EMPLOYMENT_TYPES.map((t) => (
                    <MenuItem key={t} value={t}>
                      {t}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>

              <FormControl fullWidth>
                <InputLabel id="work-format-label">Формат роботи</InputLabel>
                <Select
                  labelId="work-format-label"
                  name="workFormat"
                  value={formData.workFormat}
                  label="Формат роботи"
                  onChange={handleChange}
                >
                  {WORK_FORMATS.map((f) => (
                    <MenuItem key={f} value={f}>
                      {f}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>

              <FormControl fullWidth>
                <InputLabel id="status-label">Статус</InputLabel>
                <Select
                  labelId="status-label"
                  name="status"
                  value={formData.status}
                  label="Статус"
                  onChange={handleChange}
                >
                  {STATUSES.map((s) => (
                    <MenuItem key={s} value={s}>
                      {s}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Stack>

            <Stack direction={{ xs: "column", sm: "row" }} spacing={2}>
              <TextField
                label="Зарплата від (₴)"
                name="salaryMin"
                type="number"
                value={formData.salaryMin}
                onChange={handleChange}
                placeholder="20000"
                fullWidth
              />
              <TextField
                label="Зарплата до (₴)"
                name="salaryMax"
                type="number"
                value={formData.salaryMax}
                onChange={handleChange}
                placeholder="50000"
                fullWidth
              />
            </Stack>

            <TextField
              label="Опис вакансії *"
              name="description"
              value={formData.description}
              onChange={handleChange}
              placeholder="Опишіть обов'язки, вимоги до кандидатів та умови праці..."
              multiline
              rows={6}
              fullWidth
              required
            />

            <Box sx={{ display: "flex", gap: 2, pt: 2 }}>
              <Button
                type="submit"
                variant="contained"
                size="large"
                disabled={submitting}
                startIcon={submitting && <CircularProgress size={20} color="inherit" />}
              >
                {submitting ? "Створення..." : "Опублікувати вакансію"}
              </Button>

              <Button
                variant="outlined"
                size="large"
                onClick={() => navigate(-1)}
                disabled={submitting}
              >
                Скасувати
              </Button>
            </Box>
          </Stack>
        </Box>
      </Paper>
    </Container>
  );
}
