import { useEffect } from "react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { faPen } from "@fortawesome/free-solid-svg-icons";
import { faTrashCan } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";
import { faCircleXmark } from "@fortawesome/free-solid-svg-icons";

function Plans() {
  const [data, setData] = useState(null);
  const [error, setError] = useState("");
  const [plansCount, setPlansCount] = useState(0);
  const [refreshData, setRefreshData] = useState(false);
  const [addPlanPgae, setAddPlanPage] = useState(false);
  const [inputError, setInputError] = useState("");
  const [newPlan, setNewPlan] = useState({
    name: "",
    durationInMonths: "",
    price: "",
  });

  const navigate = useNavigate();
  useEffect(() => {
    async function fetchData() {
      try {
        const response = await fetch("https://localhost:40443/plans", {
          method: "GET",

          headers: {
            "Content-Type": "application/json",
          },
          credentials: "include",
        });
        if (response.ok) {
          var result = await response.json();

          setData(result.data);
          setPlansCount(result.recordCount);

          // console.log(result.data);
        } else if (response.status === 401) {
          navigate("/account/login");
        }
      } catch (err) {
        setError(err.message);
        console.error(err);
      }
    }

    fetchData();
    console.log(data);
  }, [refreshData]);

  const handleDelete = async (e) => {
    var planId = e.currentTarget.parentElement.id;
    try {
      const response = await fetch(`https://localhost:40443/plans/${planId}`, {
        method: "DELETE",

        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
      });
      if (response.ok) {
        var result = await response.json();
        console.log(result.message);
      } else if (response.status === 401) {
        navigate("/account/login");
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }
    setRefreshData((prev) => !prev);
  };
  const handleCancel = () => {
    setAddPlanPage((prev) => !prev);
    clearPlanObject();
  };

  const validateInputs = () => {
    if (!newPlan.name || !newPlan.durationInMonths || !newPlan.price) {
      setInputError("Plase fill out all fields");
      return false;
    }
    setInputError("");
    return true;
  };
  const clearPlanObject = () => {
    setNewPlan({ name: "", durationInMonths: "", price: "" });
  };
  const handleAddNewPlan = async () => {
    if (!validateInputs()) return;

    try {
      const response = await fetch(`https://localhost:40443/plans`, {
        method: "POST",

        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newPlan),
        credentials: "include",
      });
      console.log(JSON.stringify(newPlan));
      var result = await response.json();
      if (response.ok) {
        console.log(result.message);
      } else if (response.status === 401) {
        navigate("/account/login");
      } else if (!response.ok) {
        console.log(result);
      }
    } catch (err) {
      setError(err.message);
      console.error(err);
    }

    setAddPlanPage((prev) => !prev);
    setRefreshData((prev) => !prev);
    clearPlanObject();
  };

  return (
    <>
      {data ? (
        <>
          <div className="members-main">
            <h4 style={{ fontSize: "13px" }}>All Plans ({plansCount})</h4>
            <div className="members-list">
              <div className="search-box">
                <div
                  className="add-plan"
                  style={{ cursor: "pointer" }}
                  onClick={() => setAddPlanPage((prev) => !prev)}
                >
                  <FontAwesomeIcon
                    icon={faPlus}
                    style={{
                      color: "white",
                      cursor: "pointer",
                      marginRight: "2px",
                    }}
                  />
                  New Plan
                </div>
              </div>
              {plansCount === 0 ? (
                <div className="no-members">No Plans Found</div>
              ) : (
                <table>
                  <thead>
                    <tr>
                      <th>Id</th>
                      <th>Name</th>
                      <th>Duration In Months</th>
                      <th>Price</th>
                      <th>operations</th>
                    </tr>
                  </thead>
                  <tbody>
                    {data.map((plan, index) => (
                      <tr key={index}>
                        <td>{plan.id}</td>
                        <td>{plan.name}</td>
                        <td>{plan.durationInMonths}</td>
                        <td>{plan.price} dh</td>
                        <td className="operations-row" id={plan.id}>
                          <FontAwesomeIcon
                            icon={faTrashCan}
                            style={{ color: "blue", cursor: "pointer" }}
                            onClick={handleDelete}
                          />
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </div>
          </div>
        </>
      ) : (
        <div className="spinner-container">
          <div className="spinner"></div>
        </div>
      )}

      {addPlanPgae && (
        <>
          <div className="light-layer"></div>

          <div className="add-new-member add-new-plan">
            <FontAwesomeIcon
              icon={faCircleXmark}
              className="close"
              onClick={handleCancel}
            />

            <p className="header">Add New plan</p>

            {/* </div> */}

            <div style={{ padding: "12px", fontSize: "10px" }}>
              <br></br>

              <label for="name">Name:</label>
              <input
                id="name"
                type="text"
                name="name"
                placeholder="Name"
                required
                value={newPlan.name}
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setNewPlan((prevVars) => ({
                    ...prevVars,
                    name: value,
                  }));
                }}
              ></input>

              <br></br>

              <label for="durationInMonths">Duration In Months</label>
              <input
                id="durationInMonths"
                type="number"
                name="durationInMonths"
                required
                placeholder="Months"
                value={newPlan.durationInMonths}
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setNewPlan((prevVars) => ({
                    ...prevVars,
                    durationInMonths: value,
                  }));
                }}
              ></input>

              <br></br>

              <label for="price">Price:</label>
              <input
                id="price"
                type="number"
                name="price"
                placeholder="Price"
                required
                // value={currentMember.phone}
                onChange={(e) => {
                  const { value } = e.currentTarget || { value: "" }; // Fallback to empty string
                  setNewPlan((prevVars) => ({
                    ...prevVars,
                    price: value,
                  }));
                }}
              ></input>

              <br></br>

              <br></br>
            </div>
            {inputError ? (
              <div
                style={{
                  color: "red",
                  fontSize: "10px",
                  textAlign: "center",
                  marginBottom: "10px",
                  marginTop: "-10px",
                }}
              >
                {/* {" "} */}
                {inputError}
              </div>
            ) : (
              ""
            )}
            <div className="add-buttons margin-top">
              <button className="cancel" onClick={handleCancel}>
                Cancel
              </button>
              <button className="save" onClick={handleAddNewPlan}>
                Add
              </button>
              {/* {!notAdded && (
                <FontAwesomeIcon
                  icon={faCircleXmark}
                  style={{ color: "red", fontSize: "10px" }}
                />
              )} */}
            </div>
          </div>
        </>
      )}
    </>
  );
}
export default Plans;
