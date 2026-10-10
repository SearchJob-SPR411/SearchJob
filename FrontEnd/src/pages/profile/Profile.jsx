import { useEffect, useState } from "react";
import {
  Alert,
  Avatar,
  Box,
  Button,
  CircularProgress,
  Container,
  Divider,
  Paper,
  Stack,
  Typography,
} from "@mui/material";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import MailOutlineIcon from "@mui/icons-material/MailOutlineOutlined";
import PhoneOutlinedIcon from "@mui/icons-material/PhoneOutlined";
import CalendarMonthOutlinedIcon from "@mui/icons-material/CalendarMonthOutlined";
import BadgeOutlinedIcon from "@mui/icons-material/BadgeOutlined";
import { Link } from "react-router-dom";
import { api } from "../../api";

function getInitials(firstName, lastName) {
  return `${firstName?.[0] || ""}${lastName?.[0] || ""}`.toUpperCase();
}

export default function Profile() {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    let active = true;
    const token = localStorage.getItem("token") || localStorage.getItem("authToken");

    if (!token) {
      setError("Sign in to view your profile.");
      setLoading(false);
      return () => {
        active = false;
      };
    }

    api
      .get("Users/me", {
        headers: { Authorization: `Bearer ${token}` },
      })
      .then((response) => {
        if (active) setUser(response.data);
      })
      .catch((requestError) => {
        if (!active) return;
        setError(
          requestError.response?.status === 401
            ? "Your session has expired. Sign in again to view your profile."
            : "We could not load your profile. Please try again later."
        );
      })
      .finally(() => {
        if (active) setLoading(false);
      });

    return () => {
      active = false;
    };
  }, []);

  if (loading) {
    return (
      <Container maxWidth="md" sx={{ py: 10, display: "grid", placeItems: "center" }}>
        <CircularProgress aria-label="Loading profile" />
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="sm" sx={{ py: { xs: 7, md: 12 } }}>
        <Alert severity={error.startsWith("Sign in") || error.startsWith("Your session") ? "info" : "error"}>
          {error}
        </Alert>
        <Button component={Link} to="/" startIcon={<ArrowBackIcon />} sx={{ mt: 3 }}>
          Browse vacancies
        </Button>
      </Container>
    );
  }

  const fullName = `${user.firstName} ${user.lastName}`.trim();
  const details = [
    { label: "Email address", value: user.email, icon: <MailOutlineIcon /> },
    { label: "Phone number", value: user.phoneNumber || "Not added", icon: <PhoneOutlinedIcon /> },
    { label: "Account type", value: user.role, icon: <BadgeOutlinedIcon /> },
    {
      label: "Member since",
      value: user.createdAt
        ? new Date(user.createdAt).toLocaleDateString(undefined, {
            year: "numeric",
            month: "long",
            day: "numeric",
          })
        : "Recently joined",
      icon: <CalendarMonthOutlinedIcon />,
    },
  ];

  return (
    <Box sx={{ flexGrow: 1, pb: { xs: 6, md: 10 } }}>
      <Box
        sx={{
          borderBottom: "1px solid",
          borderColor: "divider",
          background: "linear-gradient(120deg, rgba(35,236,153,0.11), transparent 58%)",
        }}
      >
        <Container maxWidth="md" sx={{ py: { xs: 5, md: 7 } }}>
          <Typography variant="overline" color="primary" sx={{ fontWeight: 700 }}>
            ACCOUNT
          </Typography>
          <Typography variant="h3" sx={{ mt: 0.5, fontWeight: 700, fontSize: { xs: 34, md: 44 } }}>
            Your profile
          </Typography>
          <Typography color="text.secondary" sx={{ mt: 1 }}>
            Your SearchJob account details, all in one place.
          </Typography>
        </Container>
      </Box>

      <Container maxWidth="md" sx={{ mt: { xs: 3, md: 5 } }}>
        <Paper
          elevation={0}
          sx={{
            p: { xs: 2.5, md: 4 },
            border: "1px solid",
            borderColor: "divider",
            borderRadius: 2,
          }}
        >
          <Stack direction={{ xs: "column", sm: "row" }} spacing={2.5} alignItems={{ xs: "flex-start", sm: "center" }}>
            <Avatar
              sx={{
                width: 76,
                height: 76,
                bgcolor: "primary.main",
                color: "background.default",
                fontSize: 27,
                fontWeight: 700,
              }}
            >
              {getInitials(user.firstName, user.lastName)}
            </Avatar>
            <Box sx={{ minWidth: 0 }}>
              <Typography variant="h5" sx={{ fontWeight: 700, overflowWrap: "anywhere" }}>
                {fullName || "SearchJob member"}
              </Typography>
              <Typography color="text.secondary" sx={{ mt: 0.5 }}>
                {user.role} account
              </Typography>
            </Box>
          </Stack>

          <Divider sx={{ my: 3.5 }} />

          <Typography variant="h6" sx={{ mb: 2, fontWeight: 650 }}>
            Account details
          </Typography>
          <Box
            sx={{
              display: "grid",
              gridTemplateColumns: { xs: "1fr", sm: "repeat(2, minmax(0, 1fr))" },
              columnGap: 4,
              rowGap: 3,
            }}
          >
            {details.map((detail) => (
              <Stack key={detail.label} direction="row" spacing={1.5} alignItems="flex-start">
                <Box sx={{ color: "primary.main", display: "flex", mt: 0.2 }}>
                  {detail.icon}
                </Box>
                <Box sx={{ minWidth: 0 }}>
                  <Typography variant="body2" color="text.secondary">
                    {detail.label}
                  </Typography>
                  <Typography sx={{ mt: 0.4, fontWeight: 500, overflowWrap: "anywhere" }}>
                    {detail.value}
                  </Typography>
                </Box>
              </Stack>
            ))}
          </Box>
        </Paper>
      </Container>
    </Box>
  );
}
