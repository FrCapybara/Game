using Godot;

public partial class Orc : CharacterBody2D
{
    [Export] private int _speed = 100; // Vitesse de l'ennemi
    [Export] private AnimatedSprite2D _sprite; // recuperation de la sprite annimée
    [Export] private int _attackRange = 66; // distance d'attaque
    [Export] private int _minDistance = 65; // distance de rapprochement de l'orc 
    [Export] private int _detectionRange = 650; // distance de détection
    [Export] private int _patrolDistance = 100; // distance de patrouille

    private Vector2 _movementInput = Vector2.Zero;
    private string _lastDirection = "right"; // varibale pour stocker la dérniere direction ou l'ennemi regardait 
    private Player _target; // le joueur que l'orc va attaquer
    private bool _isAttacking = false; // varible pour indiquer si l'orc attaque ou pas 
    private Timer _attackCooldownTimer; // temps pour gerer le temps d'attaque
    private float _currentPatrolDistance = 0.0f; // distance de patrouille
    private bool _isPatrolling = false; // variable pour indiquer si l'ennemi est entrain de patrouiller
    private Vector2 _patrolDirection = Vector2.Right;

    private int notvisible = 1500;

    public override void _Ready()
    {
        _target = GetNodeOrNull<Player>("/root/GameSolo/Node2D/Player");// permet de trouver le Player dans la scène
        
        //Timer
        _attackCooldownTimer = new Timer();
        AddChild(_attackCooldownTimer);
        _attackCooldownTimer.WaitTime = 0.6; // Délai de 1 seconde entre les attaques
        _attackCooldownTimer.OneShot = true; // Le Timer ne se répète pas
        _attackCooldownTimer.Timeout += OnAttackCooldownTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target == null || _isAttacking) return;
        
        Vector2 targetPosition = _target.GlobalPosition;// recuperation de la position du joueur a chque frame

        // calcul pour la distance verticale et horizontale entre le joueur et l'ennemi
        float distanceX = Mathf.Abs(targetPosition.X - GlobalPosition.X);
        float distanceY = Mathf.Abs(targetPosition.Y - GlobalPosition.Y);

        // si le joueur n'est pas dans le zone alors on lance la fonction pour patrouiller
        if (distanceX > notvisible || distanceY > notvisible)
        {
            return;
        }
        if (distanceX > _detectionRange || distanceY > _detectionRange)
        {
            Patrol(delta);
            return;
        }

        // calcul de la direction vers le joueur
        Vector2 direction = (targetPosition - GlobalPosition).Normalized();
        float distanceToTarget = GlobalPosition.DistanceTo(targetPosition);

        // se deplacer vers le joueur mais si trop proche alors on s'arrete
        if (distanceToTarget > _minDistance)
        {
            _movementInput = direction;
        }
        else
        {
            _movementInput = Vector2.Zero; // on s'arrete si trop proche
        }

        // mouvement
        Velocity = _movementInput * _speed;
        MoveAndSlide();

        //cette partie gére les animations de mouvement
        if (direction.X > 0)
        {
            _lastDirection = "right";
            _sprite.Play("walk_right");
        }
        else if (direction.X < 0)
        {
            _lastDirection = "left";
            _sprite.Play("walk_left");
        }
        else
        {
            // si il ne bouge pas il reste sur la premiere frame 
            _sprite.Stop();
            _sprite.Frame = 0; // premiere frame
        }

        // si le joueur entre dans la zone d'attaque alors l'orc l'attaque
        if (distanceToTarget <= _attackRange && !_isAttacking)
        {
            Attack();
        }
    }

    private void Patrol(double delta) // cette fonction est la fonction de patrouille de l'orc
    {
        Velocity = _patrolDirection * _speed;
        MoveAndSlide();

        // met a jour la distance parcourue en patrouille
        _currentPatrolDistance += _speed * (float)delta;

        // cette partie permet de gerer les annimations lors de la patrouille de l'orc
        if (_patrolDirection.X > 0)
        {
            _sprite.Play("walk_right");
        }
        else
        {
            _sprite.Play("walk_left");
        }

        // si on atteint la distance de patrouille maximale alors on inverse le sens de la patrouille
        if (_currentPatrolDistance >= _patrolDistance)
        {
            _currentPatrolDistance = 0; // on reinitialise la varibale
            _patrolDirection = -_patrolDirection; // on inverse la direction
        }
    }

    private void Attack()
    {
        _isAttacking = true; // varibale a true pour dire que il attaque 

        // animations d'attaque en fonction du dernier endroit ou il regardait 
        if (_lastDirection == "right")
        {
            _sprite.Play("attack_right");
        }
        else
        {
            _sprite.Play("attack_left");
        }

        // lance le timer pour attaquer
        _attackCooldownTimer.Start();
    }

    private void OnAttackCooldownTimeout()
    {
        _isAttacking = false; // on remet l'état d'attauqe a false pour dire que on attaque plus 
    }
}