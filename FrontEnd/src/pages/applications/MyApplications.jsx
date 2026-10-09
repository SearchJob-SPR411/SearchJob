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
} from "@mui/material";
import { Link } from "react-router-dom";
import AssignmentIcon from "@mui/icons-material/Assignment";
import { getMyApplications, getLocalApplications } from "../../services/applicationService";

const getStatusColor = (status) => {
  switch (status?.toLowerCase()) {
    case "accepted":
      return "success";
    case "rejected":
      return "error";
    case "reviewed":
      return "info";
    case "pending":
    default:
      return "warning";
  }
};

const getStatusLabel = (status) => {
  switch (status?.toLowerCase()) {
    case "accepted":
      return "Прийнято";
    case "rejected":
      return "Відхилено";
    case "reviewed":
      return "На розгляді";
    case "pending":
    default:
      return "Очікує";
  }
};

export default function MyApplications() {
  const [applications, setApplications] = useState([]);
  const [loading, setLoading] = useState(true);

  const loadData = async () => {
    setLoading(true);
    try {
      const data = await getMyApplications();
      setApplications(data || []);
    } catch {
      setApplications(getLocalApplications());
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();

    const handleUpdate = () => {
      setApplications(getLocalApplications());
    };

    window.addEventListener("applications-changed", handleUpdate);
    return () => {
      window.removeEventListener("applications-changed", handleUpdate);
    };
  }, []);

  return (
    <Box sx={{ flexGrow: 1, py: 5 }}>
      <Container maxWidth="lg">
        <Box sx={{ display: "flex", alignItems: "center", gap: 1.5, mb: 1 }}>
          <AssignmentIcon color="primary" sx={{ fontSize: 36 }} />
          <Typography variant="h3" fontWeight="bold">
            Мої заявки
          </Typography>
        </Box>
        <Typography color="text.secondary" sx={{ mb: 4 }}>
          Історія ваших відгуків на вакансії та їхні поточні статуси
        </Typography>

        {loading ? (
          <Typography color="text.secondary">Завантаження заявок...</Typography>
        ) : applications.length === 0 ? (
          <Paper sx={{ p: 5, textAlign: "center", borderRadius: 2 }}>
            <Typography variant="h5" gutterBottom>
              Ви ще не подавали заявок на жодну вакансію
            </Typography>
            <Typography color="text.secondary" sx={{ mb: 3 }}>
              Перегляньте актуальні вакансії та надішліть свій перший відгук роботодавцям!
            </Typography>
            <Button component={Link} to="/" variant="contained">
              Переглянути вакансії
            </Button>
          </Paper>
        ) : (
          <Stack spacing={2.5}>
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
                    <Typography variant="h5" fontWeight="medium">
                      {app.vacancyTitle || `Вакансія #${app.vacancyId}`}
                    </Typography>
                    <Typography color="primary" sx={{ mt: 0.5 }}>
                      {app.companyName || "Компанія"}
                    </Typography>
                    {app.location && (
                      <Typography color="text.secondary" variant="body2" sx={{ mt: 0.5 }}>
                        {app.location}
                      </Typography>
                    )}
                  </Box>

                  <Box sx={{ textAlign: { xs: "left", sm: "right" } }}>
                    <Chip
                      label={getStatusLabel(app.status)}
                      color={getStatusColor(app.status)}
                      variant="filled"
                      sx={{ fontWeight: "bold" }}
                    />
                    <Typography variant="caption" color="text.secondary" display="block" sx={{ mt: 1 }}>
                      {app.appliedAt ? new Date(app.appliedAt).toLocaleDateString() : "Нещодавно"}
                    </Typography>
                  </Box>
                </Box>

                {app.coverLetter && (
                  <>
                    <Divider sx={{ my: 2 }} />
                    <Typography variant="body2" color="text.secondary" sx={{ fontStyle: "italic" }}>
                      «{app.coverLetter}»
                    </Typography>
                  </>
                )}

                <Box sx={{ mt: 2, display: "flex", gap: 2 }}>
                  <Button
                    component={Link}
                    to={`/vacancies/${app.vacancyId}`}
                    variant="outlined"
                    size="small"
                  >
                    Переглянути вакансію
                  </Button>
                </Box>
              </Paper>
            ))}
          </Stack>
        )}
      </Container>
    </Box>
  );
}
