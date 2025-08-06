import {
  BrowserRouter as Router,
  NavLink,
  Outlet,
  useNavigate,
} from "react-router-dom";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUsers } from "@fortawesome/free-solid-svg-icons";
import { faCalendarWeek } from "@fortawesome/free-solid-svg-icons/faCalendarWeek";
import { faChartPie } from "@fortawesome/free-solid-svg-icons/faChartPie";

function Dashboard() {
  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      const response = await fetch("https://localhost:40443/account/logout", {
        method: "POST",

        headers: {
          "Content-Type": "application/json",
        },

        credentials: "include",
      });
      if (response.ok) {
        navigate("/account/login");
      } else {
        console.log("login failed");
      }
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="dashboard">
      <nav className="navbar">
        <p className="name">Nassim</p>
        <ul>
          <li>
            <NavLink
              to="/dashboard"
              end
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              {({ isActive }) => (
                <>
                  <FontAwesomeIcon
                    icon={faChartPie}
                    style={{ color: isActive ? "#1e274e" : "gray" }}
                  />
                  <span>Summary</span>
                </>
              )}
            </NavLink>
          </li>
          <li>
            <NavLink
              to="members"
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              {({ isActive }) => (
                <>
                  <FontAwesomeIcon
                    icon={faUsers}
                    style={{ color: isActive ? "#1e274e" : "gray" }}
                  />
                  <span>Members</span>
                </>
              )}
            </NavLink>
          </li>
          <li>
            <NavLink
              to="plans"
              className={({ isActive }) => (isActive ? "active" : "")}
            >
              {({ isActive }) => (
                <>
                  <FontAwesomeIcon
                    icon={faCalendarWeek}
                    style={{ color: isActive ? "#1e274e" : "gray" }}
                  />
                  <span>Plans</span>
                </>
              )}
            </NavLink>
          </li>
          <button
            onClick={handleLogout}
            style={{ color: "white", backgroundColor: "red", padding: "2px" }}
          >
            Logout
          </button>
        </ul>
      </nav>

      <div className="right-side" style={{ width: "80%", padding: "20px" }}>
        <Outlet />
      </div>
    </div>
  );
}

export default Dashboard;
