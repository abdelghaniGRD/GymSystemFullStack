import { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser } from "@fortawesome/free-solid-svg-icons";
import { faUnlock } from "@fortawesome/free-solid-svg-icons";
import { redirect, useNavigate } from "react-router-dom";

function Login() {
  const [UserName, setUserName] = useState("");
  const [Password, setPassword] = useState("");
  const [Error, setError] = useState("");
  const navigate = useNavigate();

  const HandleLogin = async () => {
    try {
      const response = await fetch("https://localhost:40443/account/login", {
        method: "POST",

        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ UserName, Password }),
        credentials: "include",
      });
      if (response.ok) {
        navigate("/dashboard");
      } else {
        setError("login failed");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }
  };
  return (
    <div className="login">
      <h3>Login</h3>
      <label>
        <FontAwesomeIcon icon={faUser} style={{ color: "#009dff" }} />
        <input
          placeholder="UserName"
          onChange={(e) => setUserName(e.target.value)}
        ></input>
      </label>

      <label>
        <FontAwesomeIcon icon={faUnlock} style={{ color: "#009dff" }} />

        <input
          placeholder="Password"
          type="password"
          onChange={(e) => setPassword(e.target.value)}
        ></input>
      </label>
      <button
        onClick={HandleLogin}
        className="login-button"
        style={{ cursor: "pointer" }}
      >
        Login
      </button>
      {Error && <p style={{ color: "red", fontWeight: "500" }}>{Error}</p>}
    </div>
  );
}

export default Login;
