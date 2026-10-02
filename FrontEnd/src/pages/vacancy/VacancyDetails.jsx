import { useEffect, useState } from "react";
import {
    Box,
    Button,
    Container,
    Typography,
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../../api";

export default function VacancyDetails() {
    const { id } = useParams();
    const navigate = useNavigate();

    const companyId = new URLSearchParams(window.location.search).get("companyId");

    const [vacancy, setVacancy] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchVacancy = async () => {
            try {
                const response = await api.get(`Vacancies/${id}`);
                setVacancy(response.data);
            } catch (error) {
                console.error("Failed to load vacancy:", error);
                setError("Failed to load vacancy. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        fetchVacancy();
    }, [id]);

    if (loading) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="text.secondary">
                    Loading vacancy...
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
                    Vacancy not found.
                </Typography>
            </Container>
        );
    }

    return (
        <Box sx={{ flexGrow: 1 }}>
            <Container maxWidth="md" sx={{ py: 5 }}>
                <Typography variant="h3">
                    {vacancy.title}
                </Typography>

                <Typography
                    variant="h6"
                    color="primary"
                    sx={{ mt: 1 }}
                >
                    {vacancy.companyName}
                </Typography>

                <Typography
                    color="text.secondary"
                    sx={{ mt: 2 }}
                >
                    {vacancy.location} &nbsp; • &nbsp;
                    {vacancy.employmentType} &nbsp; • &nbsp;
                    {vacancy.workFormat}
                </Typography>

                <Typography variant="h6" sx={{ mt: 3 }}>
                    {vacancy.salaryMin !== null || vacancy.salaryMax !== null
                        ? `${vacancy.salaryMin ?? "—"} – ${vacancy.salaryMax ?? "—"} ₴`
                        : "Salary not specified"}
                </Typography>

                <Typography variant="h5" sx={{ mt: 5, mb: 2 }}>
                    Description
                </Typography>

                <Typography>
                    {vacancy.description}
                </Typography>

                <Button
                    variant="outlined"
                    onClick={() => navigate(companyId ? `/companies/${companyId}` : "/")}
                    sx={{ mt: 5, mb: 3 }}
                >
                    Back
                </Button>
            </Container>
        </Box>
    );
}