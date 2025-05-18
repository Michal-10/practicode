

import { useEffect, useState } from "react"
import { getPortfolio, searchRepositories } from "../api/github"
import SearchForm from "./SearchForm"
import PortfolioList from "./PortfolioList"
import { Box, Container, Typography, CircularProgress, Paper } from "@mui/material"
import GitHubIcon from "@mui/icons-material/GitHub"

type Repo = {
  id: number
  name: string
  description: string
  html_url: string
  stargazers_count: number
  language: string
}

export default function GitHubPortfolio() {
  const [repos, setRepos] = useState<Repo[]>([])
  const [loading, setLoading] = useState<boolean>(true)

  const loadPortfolio = async () => {
    console.log("Loading portfolio...");
    
    setLoading(true)
    try {
      console.log("-------------------------");
      
      const res = await getPortfolio()
      console.log("Portfolio data:", res.data)
      setRepos(res.data)
    } catch (err) {
      console.error("Error loading portfolio:", err)
    }
    setLoading(false)
  }

  useEffect(() => {
    loadPortfolio()
  }, [])

  const handleSearch = async (params: {
    name?: string
    language?: string
    user?: string
  }) => {
    setLoading(true)
    try {
      const res = await searchRepositories(params)
      setRepos(res.data)
    } catch (err) {
      console.error("Error searching:", err)
    }
    setLoading(false)
  }

  // Function to view all repositories
  const handleViewAll = () => {
    loadPortfolio()
  }

  return (
    <Box sx={{ minHeight: "100vh", bgcolor: "#f8f5ff", py: 4 }}>
      <Container maxWidth="lg">
        <Paper
          elevation={3}
          sx={{
            borderRadius: 3,
            overflow: "hidden",
            boxShadow: "0 10px 30px rgba(138, 51, 253, 0.1)",
          }}
        >
          <Box
            sx={{
              p: 3,
              textAlign: "center",
              background: "linear-gradient(90deg, #8a33fd, #d161ff)",
              color: "white",
              mb: 3,
            }}
          >
            <Box sx={{ display: "flex", alignItems: "center", justifyContent: "center", mb: 1 }}>
              <GitHubIcon sx={{ fontSize: 40, mr: 1 }} />
              <Typography variant="h4" component="h1" fontWeight="bold">
                GitHub Portfolio
              </Typography>
            </Box>
            <Typography variant="subtitle1">Search and discover interesting repositories on GitHub</Typography>
          </Box>

          <Box sx={{ px: 3, pb: 3 }}>
            <SearchForm onSearch={handleSearch} onViewAll={handleViewAll} />

            <Box sx={{ mt: 4, minHeight: "300px" }}>
              {loading ? (
                <Box sx={{ display: "flex", justifyContent: "center", alignItems: "center", height: "300px" }}>
                  <CircularProgress size={60} thickness={4} sx={{ color: "#8a33fd" }} />
                </Box>
              ) : (
                <PortfolioList repos={repos} />
              )}
            </Box>
          </Box>
        </Paper>
      </Container>
    </Box>
  )
}
