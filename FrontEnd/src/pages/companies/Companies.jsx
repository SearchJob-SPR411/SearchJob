import { useEffect, useState } from "react";
import {
    Box,
    Button,
    Container,
    Typography,
} from "@mui/material";
import { Link } from "react-router-dom";
import { api } from "../../api";

export default function Companies() {
    const [companies, setCompanies] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const fetchCompanies = async () => {
            try {
                const response = await api.get("Companies");
                setCompanies(response.data);
            } catch (error) {
                console.error("Failed to load companies:", error);
                setError("Failed to load companies. Please try again later.");
            } finally {
                setLoading(false);
            }
        };

        fetchCompanies();
    }, []);

    if (loading) {
        return (
            <Container sx={{ py: 5 }}>
                <Typography color="text.secondary">
                    Loading companies...
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

    return (
        <Box sx={{ flexGrow: 1 }}>
            <Container maxWidth="lg" sx={{ py: 5 }}>
                <Typography variant="h3" sx={{ mb: 4 }}>
                    Companies
                </Typography>

                <Box
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        gap: 2,
                    }}
                >
                    {companies.map((company) => (
                        <Box
                            key={company.id}
                            sx={{
                                p: 3,
                                border: 1,
                                borderColor: "divider",
                                borderRadius: 2,
                                backgroundColor: "background.paper",
                            }}
                        >
                            <Typography variant="h5">
                                {company.name}
                            </Typography>

                            <Typography
                                color="text.secondary"
                                sx={{ mt: 1 }}
                            >
                                {company.location}
                            </Typography>

                            <Typography sx={{ mt: 2 }}>
                                {company.description}
                            </Typography>

                            {company.website && (
                                <Typography
                                    color="primary"
                                    sx={{ mt: 1 }}
                                >
                                    {company.website}
                                </Typography>
                            )}

                            <Button
                                component={Link}
                                to={`/companies/${company.id}`}
                                variant="contained"
                                sx={{ mt: 2, alignSelf: "flex-start" }}
                            >
                                View company
                            </Button>
                        </Box>
                    ))}

                    {companies.length === 0 && (
                        <Typography color="text.secondary">
                            No companies found.
                        </Typography>
                    )}
                </Box>
            </Container>
        </Box>
    );
}

