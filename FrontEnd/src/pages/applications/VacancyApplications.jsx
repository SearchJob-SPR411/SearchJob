import { useEffect, useState } from "react";
import {
  Box,
  Container,
  Typography,
  Paper,
  Chip,
  Button,
  Stack,
  Divider,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Alert,
  CircularProgress,
} from "@mui/material";
import { useParams, useNavigate } from "react-router-dom";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import { api } from "../../api";
import { getVacancyApplications, updateApplicationStatus } from "../../services/applicationService";

const STATUS_OPTIONS = [
  { value: "Pending", label: "Очікує (Pending)", color: "warning" },
  { value: "Reviewed", label: "На розгляді (Reviewed)", color: "info" },
  { value: "Accepted", label: "Прийнято (Accepted)", color: "success" },
  { value: "Rejected", label: "Відхилено (Rejected)", color: "error" },
];

export default function VacancyApplications() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [vacancy, setVacancy] = useState(null);
  const [applications, setApplications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [statusUpdating, setStatusUpdating] = useState({});
  const [notice, setNotice] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [vacRes, apps] = await Promise.all([
          api.get(`Vacancies/${id}`).catch(() => null),
          getVacancyApplications(id),
        ]);
        if (vacRes?.data) {
          setVacancy(vacRes.data);
        }
        setApplications(apps || []);
      } catch (err) {
        console.error("Failed to load vacancy applications", err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleStatusChange = async (appId, newStatus) => {
    setStatusUpdating((prev) => ({ ...prev, [appId]: true }));
    try {
      await updateApplicationStatus(appId, newStatus);
      setApplications((prev) =>
        prev.map((a) => (Number(a.id) === Number(appId) ? { ...a, status: newStatus } : a))
      );
      setNotice(`Статус заявки #${appId} оновлено на «${newStatus}»`);
      setTimeout(() => setNotice(""), 3000);
    } catch (err) {
      console.error("Status update error", err);
    } finally {
      setStatusUpdating((prev) => ({ ...prev, [appId]: false }));
    }
  };

  return (
    <Box sx={{ flexGrow: 1, py: 5 }}>
      <Container maxWidth="lg">
        <Button
          startIcon={<ArrowBackIcon />}
          onClick={() => navigate(`/vacancies/${id}`)}
          sx={{ mb: 3 }}
        >
          Назад до вакансії
        </Button>

        <Typography variant="h3" fontWeight="bold">
          Заявки кандидатів
        </Typography>

        {vacancy && (
          <Typography color="primary" variant="h6" sx={{ mt: 1, mb: 4 }}>
            Вакансія: «{vacancy.title}» ({vacancy.companyName})
          </Typography>
        )}

        {notice && (
          <Alert severity="success" sx={{ mb: 3 }}>
            {notice}
          </Alert>
        )}

        {loading ? (
          <Typography color="text.secondary">Завантаження заявок кандидатів...</Typography>
        ) : applications.length === 0 ? (
          <Paper sx={{ p: 5, textAlign: "center", borderRadius: 2 }}>
            <Typography variant="h5" gutterBottom>
              Заявок на цю вакансію поки немає
            </Typography>
            <Typography color="text.secondary">
              Коли кандидати відгукнуться на цю пропозицію, їхні анкети з'являться тут.
            </Typography>
          </Paper>
        ) : (
          <Stack spacing={3}>
            {applications.map((app) => (
              <Paper
                key={app.id}
                sx={{
                  p: 3,
                  borderRadius: 2,
                  border: 1,
                  borderColor: "divider",
                }}
              >
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "flex-start",
                    flexWrap: "wrap",
                    gap: 2,
                  }}
                >
                  <Box>
                    <Typography variant="h5" fontWeight="bold">
                      {app.applicantName || `Кандидат #${app.userId || app.id}`}
                    </Typography>
                    {app.applicantEmail && (
                      <Typography color="text.secondary" variant="body2" sx={{ mt: 0.5 }}>
                        Email: {app.applicantEmail}
                      </Typography>
                    )}
                    {app.applicantPhone && (
                      <Typography color="text.secondary" variant="body2">
                        Телефон: {app.applicantPhone}
                      </Typography>
                    )}
                    {app.resumeTitle && (
                      <Typography color="primary" variant="body2" sx={{ mt: 0.5, fontWeight: "medium" }}>
                        Прикріплене резюме: {app.resumeTitle}
                      </Typography>
                    )}
                  </Box>

                  <Box sx={{ minWidth: 200 }}>
                    <FormControl fullWidth size="small">
                      <InputLabel id={`status-label-${app.id}`}>Статус заявки</InputLabel>
                      <Select
                        labelId={`status-label-${app.id}`}
                        value={app.status || "Pending"}
                        label="Статус заявки"
                        disabled={statusUpdating[app.id]}
                        onChange={(e) => handleStatusChange(app.id, e.target.value)}
                      >
                        {STATUS_OPTIONS.map((opt) => (
                          <MenuItem key={opt.value} value={opt.value}>
                            {opt.label}
                          </MenuItem>
                        ))}
                      </Select>
                    </FormControl>

                    {statusUpdating[app.id] && (
                      <Box sx={{ display: "flex", alignItems: "center", gap: 1, mt: 1 }}>
                        <CircularProgress size={16} />
                        <Typography variant="caption">Оновлення статусу...</Typography>
                      </Box>
                    )}
                  </Box>
                </Box>

                {app.coverLetter && (
                  <>
                    <Divider sx={{ my: 2 }} />
                    <Typography variant="subtitle2" color="text.secondary" gutterBottom>
                      Супровідний лист кандидата:
                    </Typography>
                    <Typography variant="body1" sx={{ whiteSpace: "pre-line" }}>
                      {app.coverLetter}
                    </Typography>
                  </>
                )}

                <Box sx={{ mt: 2, display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                  <Typography variant="caption" color="text.secondary">
                    Подано: {app.appliedAt ? new Date(app.appliedAt).toLocaleString() : "Нещодавно"}
                  </Typography>

                  <Chip
                    label={app.status || "Pending"}
                    color={
                      app.status === "Accepted"
                        ? "success"
                        : app.status === "Rejected"
                        ? "error"
                        : app.status === "Reviewed"
                        ? "info"
                        : "warning"
                    }
                    size="small"
                  />
                </Box>
              </Paper>
            ))}
          </Stack>
        )}
      </Container>
    </Box>
  );
}
