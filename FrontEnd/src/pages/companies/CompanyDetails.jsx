
import { useEffect, useState } from "react";
import {
    Box,
    Button,
    Container,
    Typography,
} from "@mui/material";
import { Link, useNavigate, useParams } from "react-router-dom";
import { api } from "../../api";

export default function CompanyDetails() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [company, setCompany] = useState(null);
    const [vacancies, setVacancies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchCompany = async () => {
            try {
                const companyResponse = await api.get(`Companies/${id}`);
                const vacanciesResponse = await api.get(
                    `Vacancies/company/${id}`
                );

                setCompany(companyResponse.data);
                setVacancies(vacanciesResponse.data);
            } catch (error) {
                console.error("Failed to load company:", error);
                setError("Failed to load company. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        fetchCompany();
    }, [id]);

    if (loading) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="text.secondary">
                    Loading company...
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

    if (!company) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="text.secondary">
                    Company not found.
                </Typography>
            </Container>
        );
    }

    return (
        <Box sx={{ flexGrow: 1 }}>
            <Container maxWidth="md" sx={{ py: 5 }}>
                <Typography variant="h3">
                    {company.name}
                </Typography>

                <Typography
                    color="text.secondary"
                    sx={{ mt: 2 }}
                >
                    {company.location}
                </Typography>

                <Typography variant="h5" sx={{ mt: 5, mb: 2 }}>
                    Description
                </Typography>

                <Typography>
                    {company.description}
                </Typography>

                {company.website && (
                    <Typography
                        color="primary"
                        sx={{ mt: 3 }}
                    >
                        {company.website}
                    </Typography>
                )}

                <Box sx={{ display: "flex", justifyContent: "space-between", alignItems: "center", mt: 5, mb: 2 }}>
                    <Typography variant="h5">
                        Vacancies
                    </Typography>

                    <Button
                        component={Link}
                        to={`/vacancies/create?companyId=${company.id}`}
                        variant="contained"
                        size="small"
                    >
                        + Add vacancy
                    </Button>
                </Box>

                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        gap: 2,
                    }}
                >
                    {vacancies.map((vacancy) => (
                        <Box
                            key={vacancy.id}
                            sx={{
                                p: 3,
                                border: 1,
                                borderColor: "divider",
                                borderRadius: 2,
                                backgroundColor: "background.paper",
                            }}
                        >
                            <Typography variant="h6">
                                {vacancy.title}
                            </Typography>

                            <Typography
                                color="text.secondary"
                                sx={{ mt: 1 }}
                            >
                                {vacancy.location} &nbsp; • &nbsp;
                                {vacancy.employmentType} &nbsp; • &nbsp;
                                {vacancy.workFormat}
                            </Typography>

                            <Button
                                component={Link}
                                to={`/vacancies/${vacancy.id}?companyId=${company.id}`}
                                variant="contained"
                                sx={{ mt: 2 }}
                            >
                                View vacancy
                            </Button>
                        </Box>
                    ))}

                    {vacancies.length === 0 && (
                        <Typography color="text.secondary">
                            No vacancies found.
                        </Typography>
                    )}
                </Box>

                <Button
                    variant="outlined"
                    onClick={() => navigate("/companies")}
                    sx={{ mt: 5, mb: 3 }}
                >
                    Back to companies
                </Button>
            </Container>
        </Box>
    );
}

