# SushiNference-Bayes: A Bayesian Sushi Recommendation Engine

Welcome to SushiNference, a C# .NET project that implements a Bayesian inference-based sushi recommendation engine. This project leverages the power of probabilistic programming to provide personalized sushi recommendations based on user preferences and item characteristics.

## Table of Contents
- [Introduction](#introduction)
- [Dataset](#dataset)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Methodology](#methodology)
- [Results](#results)
- [Future Enhancements](#future-enhancements)
- [Contributing](#contributing)
- [License](#license)

## Introduction
SushiNference aims to enhance the sushi dining experience by offering tailored recommendations to users based on their individual tastes and the attributes of various sushi items. By employing Bayesian inference techniques, the engine learns from user rating data and item features to uncover latent factors that drive user preferences.

## Dataset
The project utilizes the "Sushi Preference Data Set" sourced from the [UC Irvine Machine Learning Repository](https://archive.ics.uci.edu/ml/datasets/Sushi+Preference+Data+Set). This dataset contains preference data from 5,000 individuals on 100 different sushi items. Each sushi item is characterized by features such as style, major group, minor group, and oiliness. The dataset also includes information on the frequency and consumption status of each item.

## Project Structure
The SushiNference project follows a modular structure to ensure code organization and maintainability. Here's an overview of the key components:

- `Program.cs`: The entry point of the application, responsible for loading data and initializing the recommendation engine.
- `SushiRecommender.cs`: The core class that implements the Bayesian collaborative filtering model and performs inference to generate recommendations.
- `DataLoader.cs`: A utility class that handles the loading of sushi item data and user preference data from the dataset files.
- `Models/`: A directory containing the data models used in the project, including `SushiItem` and `UserPreference`.

## Getting Started
To run the SushiNference project locally, follow these steps:

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/SushiNference.git
   ```

2. Open the project in your preferred C# IDE.

3. Ensure you have the necessary dependencies installed, including the Infer.NET probabilistic programming framework.

4. Build the project to restore dependencies and compile the code.

5. Run the `Program.cs` file to execute the recommendation engine.

## Methodology
SushiNference employs a Bayesian collaborative filtering approach to generate recommendations. The key steps involved are:

1. **Data Loading**: The `DataLoader` class reads the sushi item data and user preference data from the dataset files and converts them into `SushiItem` and `UserPreference` objects.

2. **Model Construction**: The `SushiRecommender` class constructs a probabilistic graphical model using Infer.NET. The model represents user factors, item factors, and the rating matrix as random variables with Gaussian distributions.

3. **Inference**: The model performs Bayesian inference to learn the latent user and item factors based on the observed user ratings. This is done using the expectation-maximization (EM) algorithm.

4. **Recommendation**: Once the model is trained, it can generate personalized recommendations for a given user by predicting the ratings for unrated items using the learned user and item factors.

## Results
The SushiNference engine demonstrates the effectiveness of Bayesian inference in capturing user preferences and generating accurate sushi recommendations. By leveraging the rich dataset and the power of probabilistic programming, the engine provides a personalized sushi experience tailored to individual tastes.

## Future Enhancements
There are several potential avenues for enhancing the SushiNference project:

- Incorporating additional item features and user demographics to improve recommendation accuracy.
- Implementing a web-based user interface for a more interactive and user-friendly experience.
- Exploring alternative recommendation algorithms and comparing their performance.
- Integrating with a sushi restaurant database to provide real-time recommendations based on menu availability.

## Contributing
Contributions to the SushiNference project are welcome! If you have any ideas, suggestions, or bug reports, please open an issue on the project's GitHub repository. If you'd like to contribute code, feel free to fork the repository and submit a pull request.

## License
The SushiNference project is licensed under the [MIT License](LICENSE). Feel free to use, modify, and distribute the code for personal or commercial purposes.

