using Shared.Models;
using LiftLab.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftLab.ViewModels
{
    public class CreateWorkoutPlanViewModel : BaseViewModel
    {
        private readonly WorkoutPlansServiceUI _workoutService;

        #region Observable Collections
        public ObservableCollection<Workouts> AvailableWorkouts { get; set; }
        public ObservableCollection<WorkoutInPlanDTO> SelectedWorkouts { get; set; }
        #endregion

        #region Variables
        private string workoutPlanName;
        private Workouts selectedWorkout;
        #endregion

        #region ICommands
        public ICommand AddWorkoutCommand { get; }
        public ICommand CreateWorkoutPlanCommand { get; }
        #endregion

        public CreateWorkoutPlanViewModel()
        {
            _workoutService = new WorkoutPlansServiceUI();
            AvailableWorkouts = new ObservableCollection<Workouts>();
            SelectedWorkouts = new ObservableCollection<WorkoutInPlanDTO>();

            AddWorkoutCommand = new Command<Workouts>(AddWorkoutToPlan);
            CreateWorkoutPlanCommand = new Command(async () => await CreateWorkoutPlanAsync());

            LoadWorkouts();
        }

        #region Properties

        public string WorkoutPlanName
        {
            get => workoutPlanName;
            set => SetProperty(ref workoutPlanName, value);
        }

        public Workouts SelectedWorkout
        {
            get => selectedWorkout;
            set
            {
                if (SetProperty(ref selectedWorkout, value) && value != null)
                {
                    AddWorkoutToPlan(value);
                }
            }
        }

        #endregion

        #region Methods

        private async void LoadWorkouts()
        {
            var workouts = await _workoutService.GetAllWorkouts();
            AvailableWorkouts.Clear();
            foreach (var workout in workouts)
            {
                AvailableWorkouts.Add(workout);
            }
        }

        private void AddWorkoutToPlan(Workouts workout)
        {
            if (workout == null)
                return;

            var workoutInPlan = new WorkoutInPlanDTO
            {
                WorkoutId = workout.WorkoutId,
                Sets = 3, // default sets
                Reps = 10 // default reps
            };

            SelectedWorkouts.Add(workoutInPlan);
        }

        private async Task CreateWorkoutPlanAsync()
        {
            int userId = Preferences.Get("UserId", 0); // pulls user from preferences

            var createWorkoutPlan = new CreateWorkoutPlan
            {
                WorkoutPlanName = WorkoutPlanName,
                UserId = userId,
                Workouts = SelectedWorkouts.ToList()
            };

            await _workoutService.CreateWorkoutPlan(createWorkoutPlan);

            await Application.Current.MainPage.DisplayAlert("Success", "Workout Plan Created!", "OK");

            await Shell.Current.GoToAsync(".."); // Navigate back after success
        }

        #endregion
    }
}
