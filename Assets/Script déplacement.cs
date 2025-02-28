using UnityEngine;
using UnityEngine.InputSystem;

public class SubmarineController : MonoBehaviour
{
    // Variables pour le mouvement
    public float moveSpeed = 5f; // Vitesse de déplacement normale
    private float currentSpeed; // Vitesse actuelle (peut changer avec Shift)
    public float rotateSpeed = 100f; // Vitesse de rotation

    // Références aux actions d'entrée
    private InputAction moveForwardAction;
    private InputAction moveBackwardAction;
    private InputAction descendAction;
    private InputAction ascendAction;
    private InputAction shiftAction; // Action pour détecter Shift

    private Rigidbody rb; // Le Rigidbody du sous-marin pour gérer la physique
    private Animator animator; // Référence à l'Animator du sous-marin

    private void Awake()
    {
        // Associer le Rigidbody pour appliquer les forces
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Récupérer le composant Animator

        // Créer une instance de InputActionMap
        var inputActions = new InputActionMap("Submarine");

        // Définir les actions pour les mouvements
        moveForwardAction = inputActions.AddAction("MoveForward", binding: "<Keyboard>/w");
        moveBackwardAction = inputActions.AddAction("MoveBackward", binding: "<Keyboard>/s");
        descendAction = inputActions.AddAction("Descend", binding: "<Keyboard>/q");
        ascendAction = inputActions.AddAction("Ascend", binding: "<Keyboard>/e");

        // Définir l'action pour la touche Shift
        shiftAction = inputActions.AddAction("Shift", binding: "<Keyboard>/shift");

        // Activer les actions
        inputActions.Enable();
        
        // Initialiser la vitesse actuelle
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        // Vérifier si la touche Shift est enfoncée pour doubler la vitesse
        if (shiftAction.ReadValue<float>() > 0.1f) // Si Shift est enfoncé
        {
            currentSpeed = moveSpeed * 2f; // Doubler la vitesse
        }
        else
        {
            currentSpeed = moveSpeed; // Sinon, vitesse normale
        }

        // Calculer le mouvement
        Vector3 movement = Vector3.zero;
        float direction = 0f; // Variable pour déterminer la direction de l'animation

        // Mouvement avant
        if (moveForwardAction.ReadValue<float>() > 0.1f)
        {
            movement += transform.forward * currentSpeed * Time.deltaTime;
            direction = 1f; // Le sous-marin se déplace vers l'avant
        }

        // Mouvement arrière
        if (moveBackwardAction.ReadValue<float>() > 0.1f)
        {
            movement -= transform.forward * currentSpeed * Time.deltaTime;
            direction = -1f; // Le sous-marin se déplace vers l'arrière
        }

        // Mouvement vers le bas (descendre)
        if (descendAction.ReadValue<float>() > 0.1f)
        {
            movement -= transform.up * currentSpeed * Time.deltaTime;
        }

        // Mouvement vers le haut (monter)
        if (ascendAction.ReadValue<float>() > 0.1f)
        {
            movement += transform.up * currentSpeed * Time.deltaTime;
        }

        // Appliquer le mouvement au Rigidbody
        rb.MovePosition(rb.position + movement);

        // Mettre à jour l'Animator
        UpdateAnimator(direction);
    }

    private void UpdateAnimator(float direction)
    {
        // Mettre à jour le paramètre "Speed" dans l'Animator pour changer la vitesse de l'animation
        animator.SetFloat("Speed", Mathf.Abs(currentSpeed)); // On utilise la valeur absolue pour éviter les valeurs négatives

        // Mettre à jour la direction dans l'Animator
        animator.SetFloat("Direction", direction); // 1 = avant, -1 = arrière, 0 = repos

        // Si le sous-marin est en repos (pas de mouvement)
        if (direction == 0f)
        {
            animator.SetBool("IsMoving", false); // Le sous-marin est immobile
        }
        else
        {
            animator.SetBool("IsMoving", true); // Le sous-marin est en mouvement
        }
    }

    private void OnDestroy()
    {
        // Désactiver les actions lorsque le script est détruit
        if (moveForwardAction != null) moveForwardAction.Disable();
        if (moveBackwardAction != null) moveBackwardAction.Disable();
        if (descendAction != null) descendAction.Disable();
        if (ascendAction != null) ascendAction.Disable();
        if (shiftAction != null) shiftAction.Disable();
    }
}
