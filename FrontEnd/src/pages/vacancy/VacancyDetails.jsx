import { useEffect, useState } from "react";
import {
    Box,
    Button,
    Container,
    Typography,
    IconButton,
    Tooltip,
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    TextField,
    Alert,
    CircularProgress,
    Stack,
    Divider,
} from "@mui/material";
import BookmarkIcon from "@mui/icons-material/Bookmark";
import BookmarkBorderIcon from "@mui/icons-material/BookmarkBorder";
import SendIcon from "@mui/icons-material/Send";
import PeopleIcon from "@mui/icons-material/People";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import { useNavigate, useParams, Link } from "react-router-dom";
import { api } from "../../api";
import { isLocalFavorite, toggleFavorite } from "../../services/favoritesService";
import { applyToJob, hasApplied } from "../../services/applicationService";

export default function VacancyDetails() {
    const { id } = useParams();
    const navigate = useNavigate();

    const companyId = new URLSearchParams(window.location.search).get("companyId");

    const [vacancy, setVacancy] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [isFav, setIsFav] = useState(false);
    const [appliedInfo, setAppliedInfo] = useState(null);

    // Apply modal state
    const [applyModalOpen, setApplyModalOpen] = useState(false);
    const [coverLetter, setCoverLetter] = useState("");
    const [resumes, setResumes] = useState([]);
    const [selectedResumeId, setSelectedResumeId] = useState("");
    const [applying, setApplying] = useState(false);
    const [applyError, setApplyError] = useState("");
    const [applySuccess, setApplySuccess] = useState("");

    useEffect(() => {
        const fetchVacancy = async () => {
            try {
                const response = await api.get(`Vacancies/${id}`);
                setVacancy(response.data);
                setIsFav(isLocalFavorite(response.data.id));
                setAppliedInfo(hasApplied(response.data.id));
            } catch (err) {
                console.error("Failed to load vacancy:", err);
                setError("Failed to load vacancy. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        fetchVacancy();
    }, [id]);

    useEffect(() => {
        if (applyModalOpen) {
            const fetchResumes = async () => {
                try {
                    const res = await api.get("Resumes");
                    setResumes(res.data || []);
                    if (res.data && res.data.length > 0) {
                        setSelectedResumeId(res.data[0].id);
                    }
                } catch {
                    // Resumes optional
                }
            };
            fetchResumes();
        }
    }, [applyModalOpen]);

    const handleToggleFavorite = async () => {
        if (!vacancy) return;
        const newState = await toggleFavorite(vacancy);
        setIsFav(newState);
    };

    const handleOpenApplyModal = () => {
        setApplyError("");
        setApplySuccess("");
        setApplyModalOpen(true);
    };

    const handleCloseApplyModal = () => {
        setApplyModalOpen(false);
    };

    const handleSubmitApplication = async () => {
        setApplyError("");
        setApplying(true);
        try {
            const result = await applyToJob({
                vacancy,
                vacancyId: vacancy.id,
                resumeId: selectedResumeId || null,
                coverLetter,
            });
            setAppliedInfo(result);
            setApplySuccess("Заявку успішно надіслано!");
            setTimeout(() => {
                setApplyModalOpen(false);
            }, 1200);
        } catch (err) {
            setApplyError(err.message || "Не вдалося надіслати заявку.");
        } finally {
            setApplying(false);
        }
    };

    if (loading) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="text.secondary">
                    Завантаження вакансії...
                </Typography>
            </Container>
        );
    }

    if (error) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="error">
                    {error}
                </Typography>
            </Container>
        );
    }

    if (!vacancy) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="text.secondary">
                    Вакансію не знайдено.
                </Typography>
            </Container>
        );
    }

    return (
        <Box sx={{ flexGrow: 1 }}>
            <Container maxWidth="md" sx={{ py: 5 }}>
                <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", gap: 2 }}>
                    <Box sx={{ flexGrow: 1 }}>
                        <Typography variant="h3" fontWeight="bold">
                            {vacancy.title}
                        </Typography>

                        <Typography
                            variant="h6"
                            color="primary"
                            sx={{ mt: 1 }}
                        >
                            {vacancy.companyName}
                        </Typography>
                    </Box>

                    <Tooltip title={isFav ? "Видалити з обраного" : "Додати в обране"}>
                        <IconButton
                            onClick={handleToggleFavorite}
                            color={isFav ? "primary" : "default"}
                            size="large"
                            sx={{ border: 1, borderColor: "divider" }}
                        >
                            {isFav ? <BookmarkIcon /> : <BookmarkBorderIcon />}
                        </IconButton>
                    </Tooltip>
                </Box>

                <Typography
                    color="text.secondary"
                    sx={{ mt: 2 }}
                >
                    {vacancy.location} &nbsp; • &nbsp;
                    {vacancy.employmentType} &nbsp; • &nbsp;
                    {vacancy.workFormat}
                </Typography>

                <Typography variant="h5" sx={{ mt: 3, fontWeight: "bold" }}>
                    {vacancy.salaryMin !== null || vacancy.salaryMax !== null
                        ? `${vacancy.salaryMin ?? "—"} – ${vacancy.salaryMax ?? "—"} ₴`
                        : "Зарплата не вказана"}
                </Typography>

                {/* Application actions bar */}
                <Box sx={{ mt: 4, p: 2.5, bgcolor: "background.paper", borderRadius: 2, border: 1, borderColor: "divider" }}>
                    <Stack direction={{ xs: "column", sm: "row" }} spacing={2} alignItems="center" justifyContent="space-between">
                        {appliedInfo ? (
                            <Box sx={{ display: "flex", alignItems: "center", gap: 1.5 }}>
                                <CheckCircleIcon color="success" />
                                <Box>
                                    <Typography fontWeight="medium">
                                        Ви подали заявку на цю вакансію
                                    </Typography>
                                    <Typography variant="body2" color="text.secondary">
                                        Статус: <strong>{appliedInfo.status || "Pending"}</strong>
                                    </Typography>
                                </Box>
                            </Box>
                        ) : (
                            <Button
                                variant="contained"
                                color="primary"
                                size="large"
                                startIcon={<SendIcon />}
                                onClick={handleOpenApplyModal}
                            >
                                Подати заявку на вакансію
                            </Button>
                        )}

                        <Button
                            component={Link}
                            to={`/vacancies/${vacancy.id}/applications`}
                            variant="outlined"
                            startIcon={<PeopleIcon />}
                        >
                            Заявки кандидатів
                        </Button>
                    </Stack>
                </Box>

                <Divider sx={{ my: 4 }} />

                <Typography variant="h5" sx={{ mb: 2 }} fontWeight="bold">
                    Опис вакансії
                </Typography>

                <Typography sx={{ whiteSpace: "pre-line", lineHeight: 1.7 }}>
                    {vacancy.description}
                </Typography>

                <Box sx={{ mt: 5, mb: 3, display: "flex", gap: 2 }}>
                    <Button
                        variant="outlined"
                        onClick={() => navigate(companyId ? `/companies/${companyId}` : "/")}
                    >
                        Назад
                    </Button>
                </Box>
            </Container>

            {/* Apply dialog */}
            <Dialog open={applyModalOpen} onClose={handleCloseApplyModal} fullWidth maxWidth="sm">
                <DialogTitle>
                    Подати заявку на «{vacancy.title}»
                </DialogTitle>
                <DialogContent dividers>
                    <Stack spacing={3} sx={{ pt: 1 }}>
                        {applyError && <Alert severity="error">{applyError}</Alert>}
                        {applySuccess && <Alert severity="success">{applySuccess}</Alert>}

                        <Typography variant="body2" color="text.secondary">
                            Компанія: <strong>{vacancy.companyName}</strong> | Локація: <strong>{vacancy.location}</strong>
                        </Typography>

                        {resumes.length > 0 && (
                            <TextField
                                select
                                label="Виберіть резюме"
                                value={selectedResumeId}
                                onChange={(e) => setSelectedResumeId(e.target.value)}
                                SelectProps={{ native: true }}
                                fullWidth
                            >
                                <option value="">Без прив'язки до резюме</option>
                                {resumes.map((r) => (
                                    <option key={r.id} value={r.id}>
                                        {r.title}
                                    </option>
                                ))}
                            </TextField>
                        )}

                        <TextField
                            label="Супровідний лист (Cover Letter)"
                            placeholder="Розкажіть коротко, чому саме ваша кандидатура підходить на цю позицію..."
                            multiline
                            rows={4}
                            value={coverLetter}
                            onChange={(e) => setCoverLetter(e.target.value)}
                            fullWidth
                        />
                    </Stack>
                </DialogContent>
                <DialogActions sx={{ p: 2 }}>
                    <Button onClick={handleCloseApplyModal} disabled={applying}>
                        Скасувати
                    </Button>
                    <Button
                        variant="contained"
                        onClick={handleSubmitApplication}
                        disabled={applying}
                        startIcon={applying && <CircularProgress size={18} color="inherit" />}
                    >
                        {applying ? "Надсилання..." : "Надіслати заявку"}
                    </Button>
                </DialogActions>
            </Dialog>
        </Box>
    );
}